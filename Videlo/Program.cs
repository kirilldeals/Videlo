using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Videlo.Configuration;
using Videlo.Data;
using Videlo.Models.Database;
using Videlo.Services;

namespace Videlo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services
                .AddDbContext<VideloContext>(options => options.UseSqlServer(connectionString));

            builder.Services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<VideloContext>();

            builder.Services
                .Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

                options.User.RequireUniqueEmail = true;
            });

            builder.Services
                .ConfigureApplicationCookie(options => options.LoginPath = "/Login");

            builder.Services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            builder.Services
                .AddSingleton<S3Service>();
            builder.Services
                .AddSingleton<UploadTaskRepository>();

            builder.Services
                .Configure<AdminCredentialsSettings>(builder.Configuration.GetSection("AdminCredentialsSettings"));

            builder.Services
                .Configure<AWSConfiguration>(builder.Configuration.GetSection("AWSConfiguration"));

            builder.Services
                .Configure<FormOptions>(o =>
                {
                    o.MultipartBodyLengthLimit = 1073741824;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "auth",
                pattern: "{action}",
                defaults: new { controller = "Auth" });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                await IdentityDataSeeder.InitializeAsync(scope.ServiceProvider);
            }

            app.Run();
        }
    }
}