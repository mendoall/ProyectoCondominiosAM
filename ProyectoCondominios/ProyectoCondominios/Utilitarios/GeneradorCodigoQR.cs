using System.Drawing.Imaging;
using System.Drawing;
using QRCoder;
using static System.Net.Mime.MediaTypeNames;

namespace ProyectoCondominios.Utilitarios
{
    public class GeneradorCodigoQR
    {
        public byte[] GenerarCodigoQR(string codigo, string nombreCondominio, IWebHostEnvironment environment)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codigo, QRCodeGenerator.ECCLevel.Q);            
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);
            return qrCodeAsPngByteArr;
        }
    }
}
