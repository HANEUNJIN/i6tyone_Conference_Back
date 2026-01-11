using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace i6tyone_Conference.Infrastructure.Utils
{
    public class QRCodeUtil
    {
        /// <summary>
        /// QR 코드 생성
        /// </summary>
        public byte[] GenerateQRCodeBytes(string text, string title)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);

            byte[] qrBytes = qrCode.GetGraphic(20);

            using var ms = new MemoryStream(qrBytes);
            using var qrBitmap = new Bitmap(ms);

            int width = qrBitmap.Width;
            int height = qrBitmap.Height + 60;

            using var finalImage = new Bitmap(width, height);
            using var g = Graphics.FromImage(finalImage);

            g.Clear(Color.White);

            // 제목 출력
            using var font = new Font("Malgun Gothic", 20, FontStyle.Bold);
            using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            var titleRect = new RectangleF(0, 0, width, 60);
            g.DrawString(title, font, Brushes.Black, titleRect, format);

            // QR 붙이기
            g.DrawImage(qrBitmap, 0, 60);

            using var output = new MemoryStream();
            finalImage.Save(output, ImageFormat.Png);
            return output.ToArray();
        }
    }
}
