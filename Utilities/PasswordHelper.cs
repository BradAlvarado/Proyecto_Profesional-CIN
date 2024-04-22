using System;
using System.Security.Cryptography;
using System.Text;

public class PasswordHelper
{
    // Método para encriptar la contraseña
    public static string EncryptPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Convertir la contraseña en un array de bytes
            byte[] bytes = Encoding.UTF8.GetBytes(password);

            // Calcular el hash
            byte[] hash = sha256.ComputeHash(bytes);

            // Convertir el hash en una cadena hexadecimal
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}