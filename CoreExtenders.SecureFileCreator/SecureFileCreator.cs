using CoreUtilities;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CoreExtenders
{
    public abstract class SecureFileCreator
    {
        protected virtual bool Compressed { get; } = false;

        protected abstract string FileExtension { get; }

        protected virtual int Iterations { get; } = 2000;

        protected abstract string Password { get; }

        protected virtual string Salt { get; } = "tH3_S@lt_01_M@ss1v3";

        private string Decrypt(string cryptoText, string password)
        {
            byte[] salt = Encoding.Unicode.GetBytes(Salt);
            byte[] cryptoBytes = Convert.FromBase64String(Compressed ? CompressionUtility.GZipCompress(cryptoText) : cryptoText);
            var aes = Aes.Create();
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            aes.Key = pbkdf2.GetBytes(32);
            aes.IV = pbkdf2.GetBytes(16);
            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cryptoBytes, 0, cryptoBytes.Length);
            }
            return Encoding.Unicode.GetString(ms.ToArray());
        }

        private string Encrypt(string plainText, string password)
        {
            byte[] salt = Encoding.Unicode.GetBytes(Salt);
            byte[] plainBytes = Encoding.Unicode.GetBytes(Compressed ? CompressionUtility.GZipCompress(plainText) : plainText);
            var aes = Aes.Create();
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            aes.Key = pbkdf2.GetBytes(32); // set a 256-bit key
            aes.IV = pbkdf2.GetBytes(16); // set a 128-bit IV
            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Load(string path)
        {
            var ext = Path.GetExtension(path).Replace(".", "");
            if (ext.ToLowerInvariant() == FileExtension.ToLowerInvariant())
            {
                var data = File.ReadAllText(path);
                return Decrypt(data, Password);
            }
            else
                return null;
        }

        public void Save(string path, string content)
        {
            var p = System.IO.Path.ChangeExtension(path, null);
            var enCnt = Encrypt(content, Password);
            var newPath = p + "." + FileExtension.ToLowerInvariant().Replace(".", "");
            File.WriteAllText(newPath, enCnt);
        }
    }
}
