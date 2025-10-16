using QRCoder;

namespace i6tyone_Conference.Infrastructure.Utils
{
    public class QRCodeUtil
    {
        /// <summary>
        /// QR 코드 PNG 파일을 생성하여 저장
        /// </summary>
        public void GenerateQRCodePngFile(string text, string fullPath)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = qrCode.GetGraphic(20);

                File.WriteAllBytes(fullPath, qrCodeBytes);
            }
        }
    }
}
