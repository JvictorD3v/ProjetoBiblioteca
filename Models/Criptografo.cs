using System;
using System.Security.Cryptography;
using System.Text;

namespace Biblioteca.Models
{
    public class Criptografo
    {
        public static string TextoCriptografado(string textoClaro)
        {
            MDS MDSHasher = MDS.Create();

            byte[] By = Encoding.Default.GetBytes(textoClaro);
            byte[] bytesCriptografado = MDSHasher.ComputeHash(By);

            StringBuilder SB = new StringBuilder();

            foreach (byte b in bytesCriptografado)
            {
                string DebugB = b.ToString("x2");
                SB.Append(DebugB);
            }

            return SB.ToString();
        }   
    }
}