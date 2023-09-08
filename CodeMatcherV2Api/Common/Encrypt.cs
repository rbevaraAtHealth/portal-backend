using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcherApiV2.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeMatcherApiV2.Common
{
    public class Encrypt
    {
        public EncDecModel EncryptString(string text)
        {
            string keyString = EncryptionDecryption.Key;

            EncDecModel enRes = new EncDecModel();
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.PKCS7;
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
                        enRes.status = 200;
                        enRes.message = "Text is Encrypted";
                        enRes.outPut = Convert.ToBase64String(result);
                        return enRes;
                    }
                }
            }
        }
    }
}
