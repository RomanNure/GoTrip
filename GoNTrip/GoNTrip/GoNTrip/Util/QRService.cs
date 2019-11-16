using System.Threading.Tasks;

using Android.Graphics;

using Java.Nio;

using Xamarin.Essentials;

using QRCodeEncoderLibrary;

using ZXing;
using ZXing.Mobile;

namespace GoNTrip.Util
{
    public class QrService
    {
        public async Task<Bitmap> Encode(string data)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.Encode(data);

            bool[,] bools = encoder.ConvertQRCodeMatrixToPixels();
            byte[] pixels = new byte[bools.Length * 4];

            int h = bools.GetLength(0);
            int w = bools.GetLength(1);

            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    int pixelNum = (i * w + j) * 4;
                    byte color = (byte)(bools[i, j] ? 0 : 255);

                    for (int c = 0; c < 3; c++)
                        pixels[pixelNum + c] = color;

                    pixels[pixelNum + 3] = 255;
                }

            Buffer buffer = ByteBuffer.Wrap(pixels).Rewind();
            Bitmap code = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888, false);
            await code.CopyPixelsFromBufferAsync(buffer);

            return code;
        }

        public async Task<string> ScanAsync()
        {
            MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions();
            MobileBarcodeScanner scanner = new MobileBarcodeScanner();

            Result result = await scanner.Scan(options);
            Vibration.Vibrate(100);

            return result == null ? "" : result.Text;
        }
    }
}
