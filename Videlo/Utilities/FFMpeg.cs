using NReco.VideoConverter;
using Videlo.Models;

namespace Videlo.Utilities
{
    public static class FFMpeg
    {
        private static readonly FFMpegConverter _ffMpeg;

        static FFMpeg()
        {
            _ffMpeg = new FFMpegConverter();
        }

        public static FormFileInfo GetVideoThumbnail(string videoURL)
        {
            var ms = new MemoryStream();
            _ffMpeg.GetVideoThumbnail(videoURL, ms);

            var imgName = $"{Guid.NewGuid()}.jpg";
            var imgInfo = new FormFileInfo(ms, imgName, "image/jpeg");
            return imgInfo;
        }
    }
}
