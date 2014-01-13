using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace RDI.Utility
{
    public class EncryptionProvider
    {
        public static string rjdKey = "";
        public static string rjdIV = "";


        /// <summary>
        /// Encrypt the text using RSA Encryption.
        /// </summary>
        /// <param name="textToEncrypt">Text to be encrypted.</param>
        /// <returns>String containing the encrypted text.</returns>
        public static string Encrypt(string textToEncrypt)
        {
            string encryptedText = null;

            RunWithRijndaelProvider(cryptoProvider =>
            {
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(cryptoProvider.Key,cryptoProvider.IV), CryptoStreamMode.Write);
                byte[] bEncrypt = (Encoding.UTF8.GetBytes(textToEncrypt));
                cs.Write(bEncrypt, 0, bEncrypt.Length);
                cs.FlushFinalBlock();
                byte[] bData = ms.ToArray();
                ms.Read(bData, 0, bData.Length);
                encryptedText = Convert.ToBase64String(bData);
            });

            return encryptedText;
        }

        /// <summary>
        /// Decrypt the text using RSA Decryption.
        /// </summary>
        /// <param name="textToDecrypt">Text to be decrypted.</param>
        /// <returns>String containing the decrypted text.</returns>
        public static string Decrypt(string textToDecrypt)
        {
            string decryptedText = null;

            RunWithRijndaelProvider(cryptoProvider =>
            {
                byte[] cipherBytes = Convert.FromBase64String(textToDecrypt);
                MemoryStream ms = new MemoryStream(cipherBytes);
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(cryptoProvider.Key, cryptoProvider.IV), CryptoStreamMode.Read);

                StreamReader reader = new StreamReader(cs);
                decryptedText = reader.ReadLine();
                ms.Close();
                cs.Close();
                reader.Close();
            });

            return decryptedText;
        }

        /// <summary>
        /// Runs the provided action with a crypto provider created using the provided
        /// public or private RSA key.
        /// </summary>
        /// <param name="key">The public or private key.</param>
        /// <param name="action">The action to run with the provider.</param>
        private static void RunWithRijndaelProvider(Action<Rijndael> action)
        {
            Rijndael rjd = Rijndael.Create();
            if (string.IsNullOrEmpty(rjdKey) || string.IsNullOrEmpty(rjdIV))
            {
                string keyFile;
                if (System.Web.HttpContext.Current == null)
                {
                    keyFile = Path.Combine(System.Environment.CurrentDirectory, "FSBEncryptionKey.xml");
                }
                else
                {
                    keyFile = System.Web.HttpContext.Current.Server.MapPath("~/FSBEncryptionKey.xml");
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(keyFile);
                XmlNodeList xnl =  xmlDoc.GetElementsByTagName("IV");
                foreach (XmlNode xn in xnl)
                {
                    rjdIV = xn.InnerText;
                }

                xnl = null;
                xnl = xmlDoc.GetElementsByTagName("Key");
                foreach (XmlNode xn in xnl)
                {
                    rjdKey = xn.InnerText;
                }
            }

            rjd.Key = Convert.FromBase64String(rjdKey);
            rjd.IV = Convert.FromBase64String(rjdIV);

            action(rjd);
        }


    }
}