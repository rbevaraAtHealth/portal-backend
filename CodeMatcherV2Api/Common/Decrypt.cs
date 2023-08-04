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
    public class Decrypt
    {
        public EncDecModel DecryptString(string cipherText)
        {
            string keyString = "E546C8DF278CD5931069B522E695D4F2";

            EncDecModel deRes = new EncDecModel();

            var fullCipher = Convert.FromBase64String(cipherText);


            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);

            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.PKCS7;
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {

                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                    deRes.status = 200;
                    deRes.message = "Text is Decrypted";
                    deRes.outPut = Convert.ToString(result);
                    return deRes;
                }
            }
        }
    }
}
