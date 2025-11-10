using QRCoder;

namespace i6tyone_Conference.Infrastructure.Utils
{
    public class QRCodeUtil
    {
        /// <summary>
        /// QR 코드 생성
        /// </summary>
        public byte[] GenerateQRCodeBytes(string text)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                return qrCode.GetGraphic(20);
            }
        }
    }
}
