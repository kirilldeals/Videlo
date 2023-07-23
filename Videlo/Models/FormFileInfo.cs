namespace Videlo.Models
{
    public class FormFileInfo : IDisposable
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        public FormFileInfo(Stream stream, string fileName, string contentType)
        {
            FileName = fileName;
            ContentType = contentType;
            Stream = stream;
        }

        public void Dispose()
        {
            Stream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
