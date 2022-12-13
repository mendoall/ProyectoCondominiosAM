using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static System.Net.WebRequestMethods;

namespace ProyectoCondominios.Utilitarios
{
    public class TransformadorDeImagenes
    {
        public byte[] ConvertirImagenABlob(IFormFile archivo)
        {
            byte[] archivoBytes = null;
            if (archivo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    archivo.CopyTo(ms);
                    archivoBytes = ms.ToArray();
                }
            }
            return archivoBytes;
        }
    }
}
