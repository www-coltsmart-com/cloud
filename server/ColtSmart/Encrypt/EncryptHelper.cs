using System;
using System.Security.Cryptography;
using System.Text;

namespace ColtSmart.Encrypt
{
    public class EncryptHelper 
    {
        private EncryptHelper() { }

        static readonly object lockHelper = new object();
        private static EncryptHelper _instance;
        public static EncryptHelper Instance
        {
            get
            {

                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new EncryptHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="sIn"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public string UnEncrypt(string sIn, string sKey = "ColtSmart")
        {
            byte[] inputByteArray = Convert.FromBase64String(sIn);
            using (System.Security.Cryptography.DESCryptoServiceProvider des = new System.Security.Cryptography.DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.Zeros;
                byte[] keyByteArray = new byte[8];
                byte[] inputKeyByteArray = ASCIIEncoding.ASCII.GetBytes(sKey);
                for (int i = 0; i < 8; i++)
                {
                    if (inputKeyByteArray.Length > i)
                        keyByteArray[i] = inputKeyByteArray[i];
                    else
                        keyByteArray[i] = 0;
                }

                des.Key = keyByteArray;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, des.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                //string str = System.Text.Encoding.Default.GetString(ms.ToArray());
                string str = System.Text.Encoding.GetEncoding("GB2312").GetString(ms.ToArray());
                //string str = System.Text.Encoding.UTF32.GetString(ms.ToArray());
                str = str.TrimEnd('\0');
                ms.Close();
                return str;
            }
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="sIn"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public string Encrypt(string sIn, string sKey = "ColtSmart")
        {
            byte[] inputByteArray = System.Text.Encoding.ASCII.GetBytes(sIn);
            using (System.Security.Cryptography.DESCryptoServiceProvider des = new System.Security.Cryptography.DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.Zeros;
                byte[] keyByteArray = new byte[8];
                byte[] inputKeyByteArray = ASCIIEncoding.ASCII.GetBytes(sKey);
                for (int i = 0; i < 8; i++)
                {
                    if (inputKeyByteArray.Length > i)
                        keyByteArray[i] = inputKeyByteArray[i];
                    else
                        keyByteArray[i] = 0;
                }

                des.Key = keyByteArray;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, des.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        public string PassEncryption(string sName, string sPassword)
        {
            return new Encrypt().PassEncryption(sName, sPassword);
        }

        /// <summary>
        /// 解析用户密码
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sPassword"></param>
        /// <returns></returns>
        public string PassDecryption(string sName,string sPassword)
        {
            return new Encrypt().PassDecryption(sName, sPassword);
        }

        public string MD5Encoding(string rawString,string salt="")
        {
            return new Encrypt().MD5Encoding(rawString, salt);
        }
    }
}
