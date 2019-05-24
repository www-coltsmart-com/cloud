using System;
using System.Security.Cryptography;
using System.Text;

namespace ColtSmart.Encrypt
{
    public class Encrypt
    {
        protected string sKey;

        public Encrypt()
        {
            this.sKey = "上海鸣驹智能科技有限公司#$%;上海鸣驹智能科技有限公司*&&^^上海鸣驹智能科技有限公司";
        }

        public string Encryption(string sStr)
        {
            var uTF = Encoding.UTF8;
            Encryption.CryptoProvider defaultProvider = ColtSmart.Encrypt.Encryption.CryptoProvider.TripleDES;
            var encryption = new Encryption(this.sKey, defaultProvider);
            string text = encryption.Encrypt(sStr);
            byte[] bytes = uTF.GetBytes(text);
            text = "";
            byte[] array = bytes;
            for (int i = 0; i < array.Length; i++)
            {
                byte b = array[i];
                string text2 = string.Format("{0:X2}", b);
                text += text2;
            }
            return text;
        }
        public string Decryption(string sStr)
        {
            int i = 0;
            Encoding uTF = Encoding.UTF8;
            byte[] array = new byte[sStr.Length / 2];
            char[] array2 = sStr.ToCharArray();
            while (i < sStr.Length)
            {
                array[i / 2] = Convert.ToByte(Uri.FromHex(array2[i]) * 16 + Uri.FromHex(array2[i + 1]));
                i += 2;
            }
            char[] array3 = new char[uTF.GetCharCount(array, 0, array.Length)];
            uTF.GetChars(array, 0, array.Length, array3, 0);
            string encryptedString = new string(array3);
            Encryption.CryptoProvider defaultProvider = ColtSmart.Encrypt.Encryption.CryptoProvider.TripleDES;
            var encryption = new Encryption(this.sKey, defaultProvider);
            return encryption.Decrypt(encryptedString);
        }
        public string PassEncryption(string sUserno, string sPass)
        {
            return this.Encryption(sUserno + "~" + sPass);
        }
        public string PassDecryption(string sUserno, string sStr)
        {
            return this.Decryption(sStr).Replace(sUserno + "~", "");
        }
        public string GetSerialno(string sOrgName, DateTime dtDateLimit, int iNumberLimit)
        {
            return this.Encryption(string.Concat(new string[]
            {
                sOrgName,
                "~",
                dtDateLimit.ToString(),
                "~",
                iNumberLimit.ToString()
            }));
        }
        public string CheckSerialno(string sSerialno, int iNumber)
        {
            string text = this.Decryption(sSerialno);
            string result = "非法用户";
            int num = text.LastIndexOf('~', 1);
            int num2 = text.LastIndexOf('~', num + 1);
            if (num2 > 1 && num > 1)
            {
                string text2 = text.Substring(num2 + 1);
                string text3 = text.Substring(num + 1, num2 - num - 1);
                if (Convert.ToInt32(text2) > iNumber && Convert.ToDateTime(text3) > DateTime.Today)
                {
                    result = text.Substring(1, num - 1);
                }
            }
            return result;
        }

        /// <summary>
        /// MD5加密
        /// 最终被加密的字符串格式：
        ///     salt不为空：rawString+salt
        ///     salt为空：rawString
        /// </summary>
        /// <param name="rawString"></param>
        /// <param name="salt"></param>
        /// <returns>32位小写</returns>
        public string MD5Encoding(string rawString, string salt)
        {
            if (rawString == null)
            {
                return null;
            }
            var rawBytes = Encoding.UTF8.GetBytes(rawString+salt??"");
            byte[] hash = MD5.Create().ComputeHash(rawBytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                // 以十六进制格式格式化 
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
