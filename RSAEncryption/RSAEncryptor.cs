using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace RSAEncryption
{
    static class RSAEncryptor
    {
        static UnicodeEncoding ByteConverter = new UnicodeEncoding();
        public static string PrivateKey { get; set; }
        public static string PublicKey { get; set; }

        public static void GenerateKey()
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                PublicKey = RSA.ToXmlString(false);
                PrivateKey = RSA.ToXmlString(true);
            }

            using (StreamWriter writer = new StreamWriter("publicKey.xml"))
            {
                writer.WriteLine(PublicKey);
            }
            using (StreamWriter writer = new StreamWriter("privateKey.xml"))
            {
                writer.WriteLine(PrivateKey);
            }
        }

        public static string LoadKey()
        {
            string key = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
                key = File.ReadAllText(openFileDialog.FileName);

            return key;
        }

        public static byte[] Encrypt(string text, bool DoOAEPPadding)
        {
            try
            {
                string key = LoadKey();
                if (key == null)
                {
                    throw new CryptographicException("key cannot be null");
                }

                byte[] DataToEncrypt = ByteConverter.GetBytes(text);
                byte[] encryptedData;

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.FromXmlString(key);

                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                MessageBox.Show(e.Message);

                return null;
            }
        }

        public static string Decrypt(byte[] DataToDecrypt, bool DoOAEPPadding)
        {
            try
            {
                string key = LoadKey();
                if (key == null)
                {
                    throw new CryptographicException("key cannot be null");
                }

                byte[] decryptedData;

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.FromXmlString(key);

                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return ByteConverter.GetString(decryptedData);
            }

            catch (CryptographicException e)
            {
                MessageBox.Show(e.ToString());

                return null;
            }
        }
    }
}
