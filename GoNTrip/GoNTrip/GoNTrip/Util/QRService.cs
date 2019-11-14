using System.IO;
using System.Threading.Tasks;

using Java.Nio;

using QRCodeEncoderLibrary;

//using QRCoder;

using Android.Graphics;
using System.Linq;

namespace GoNTrip.Util
{
    public class QRUtil
    {
        public static async Task<Bitmap> Encode(string data)
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

        public static async Task<string> Decode(Bitmap codeBmp)
        {
            return "";
        }
    }
}
