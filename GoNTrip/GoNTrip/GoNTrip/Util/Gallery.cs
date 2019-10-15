using System;
using System.IO;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;

namespace GoNTrip.Util
{
    public class Gallery
    {
        public async Task<Stream> PickPhoto(int maxSize, int quality = 100)
        {
            PickMediaOptions pickOptions = new PickMediaOptions();

            pickOptions.MaxWidthHeight = Math.Max(0, maxSize);
            pickOptions.CompressionQuality = Math.Min(Math.Max(quality, 0), 100);

            MediaFile file = await CrossMedia.Current.PickPhotoAsync(pickOptions);

            return file == null ? null : file.GetStream();
        }
    }
}
