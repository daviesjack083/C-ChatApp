﻿using System.Text;
using System.Security.Cryptography;

namespace ChatEncrypt;

public class Encryption
{
    private byte[] key;
    
    
    public Encryption()
    {
        string key = "62F0DF90CB7E513516E30F9018B92EF9";
        this.key = Encoding.UTF8.GetBytes(key);
    }


    public string EncryptMessage(String message)
    {
        byte[] encrypted;
        using (Aes aes = Aes.Create())
        {
            aes.Key = this.key;

            MemoryStream ms = new MemoryStream();
            ICryptoTransform encryptor = aes.CreateEncryptor();

            using(CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using(StreamWriter sw = new StreamWriter(cs))
                {
                    ms.Write(aes.IV);
                    sw.Write(message);
                }
            }
            encrypted = ms.ToArray();
            return Convert.ToBase64String(encrypted);
        }
    }


    public string DecryptMessage(String message)
    {
        string decrypted;
        byte[] ciphertext = Convert.FromBase64String(message);
        
        using (Aes aes = Aes.Create())
        {
            aes.Key = this.key;
            
            // Extract IV
            var iv = new byte[16];
            Buffer.BlockCopy(ciphertext, 0, iv, 0, 16);
            aes.IV = iv;

            // Extract Payload
            var payload = new byte[ciphertext.Length - 16];
            Buffer.BlockCopy(ciphertext, 16, payload, 0, ciphertext.Length - 16);

            MemoryStream ms = new MemoryStream(payload);
            ICryptoTransform decryptor = aes.CreateDecryptor();

            using(CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            {
                using(StreamReader reader = new StreamReader(cs))
                {
                    decrypted = reader.ReadToEnd();
                }  
            }
            return decrypted;
        }
    }
}
