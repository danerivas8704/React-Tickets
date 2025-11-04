using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tickets.Modelo;

namespace Tickets.Modelo.Utilidades
{
    public class Utilidades
    {
        

        public string encriptarSHA256(string strTexto)
        {
            using(SHA256 sha256Hash = SHA256.Create())
            {
                //calcular Hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(strTexto));

                //Convierte array de bytes a string
                StringBuilder builder = new StringBuilder();
                for(int i=0; i<bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("X2"));
                }

                return builder.ToString();
            }
        }     
    }
}
