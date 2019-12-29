using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyAnimeViewer.Utility
{
    /// <summary>
    /// http://www.gutgames.com/post/AES-Encryption-in-C.aspx
    /// </summary>
    public static class AES
    {
        private const string Password = "password";
        private const string Salt = "salt";
        private const string HashAlgorithm = "SHA1";
        private const int PasswordIterations = 2;
        private const string InitialVector = "initialvector";
        private const int KeySize = 256;

        public static string Encrpyt(string PlainText)
        {
            if (string.IsNullOrEmpty(PlainText))
                return "";
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] PlainTextBytes = Encoding.ASCII.GetBytes(PlainText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            byte[] CipherTextBytes = null;

            using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
            using (MemoryStream MemStream = new MemoryStream())
            using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
            {
                CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                CryptoStream.FlushFinalBlock();
                CipherTextBytes = MemStream.ToArray();
                MemStream.Close();
                CryptoStream.Close();
            }

            SymmetricKey.Clear();
            return Convert.ToBase64String(CipherTextBytes);
        }

        public static string Decrypt(string CipherText)
        {
            if (string.IsNullOrEmpty(CipherText))
                return "";
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] CipherTextBytes = Convert.FromBase64String(CipherText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
            int ByteCount = 0;

            using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes))
            using (MemoryStream MemStream = new MemoryStream(CipherTextBytes))
            using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
            {
                ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
                MemStream.Close();
                CryptoStream.Close();
            }
            SymmetricKey.Clear();
            return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);
        }
    }
}
