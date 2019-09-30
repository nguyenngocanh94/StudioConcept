using System;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            giaiMa("gbtdhjqxhdlpholvwhatgumcuxknmm", "tiigqagsbtblfwa", "yqqvzivjcyceawi");

        }

        static string giaiMa(string content, string signature, string encryptedSignature)
        {
            char[] plainText = new[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                'v', 'w', 'x', 'y', 'z'
            };
            char[] cipherText = GetCipherText(signature, encryptedSignature);
            string res = "";
            for (int i = 0; i < content.Length; i++)
            {
                int index = Array.FindIndex(cipherText, a => a == content[i]);
                res += plainText[index];
            }

            return res;

        }

        static char[] GetCipherText(string signature, string encryptSignature)
        {
            char[] plainText = new[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                'v', 'w', 'x', 'y', 'z'
            };
            char[] encryptText = new[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                'v', 'w', 'x', 'y', 'z'
            };
            for (int i = 0; i < signature.Length; i++)
            {
                //index in plaintText
                int index = Array.FindIndex(plainText, a=>a==signature[i]);
                encryptText[index] = encryptSignature[i];

            }

            int count = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                
                if (plainText[i]==encryptText[i])
                {
                    while (encryptSignature.Contains(plainText[count]))
                    {
                        count++;
                    }

                    encryptText[i] = plainText[count];
                    count++;
                }
            }

            return encryptText;
        }
    }
}
