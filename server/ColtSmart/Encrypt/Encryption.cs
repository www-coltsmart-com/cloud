
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace ColtSmart.Encrypt
{
    public class Encryption
	{
		public enum CryptoProvider
		{
			DES,
			RC2,
			TripleDES
		}
		private const string DEFAULT_KEY = "DevPower Encryption .NET";
		internal char[] searchChars = new char[]
		{
			default(char),
			'\r',
			'\n'
		};
		internal string[] replaceStrings = new string[]
		{
			"[0]",
			"[R]",
			"[N]"
		};
		private Encryption.CryptoProvider provider = Encryption.CryptoProvider.DES;
		private string classLevelKey = "DevPower Encryption .NET";
		[Description("Gets the component version number.")]
		public Version Version
		{
			get
			{
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
				return new Version(versionInfo.FileMajorPart, versionInfo.FileMajorPart, versionInfo.FileBuildPart);
			}
		}
		[Description("Gets or sets the crypto provider to use for encryption and decryption.")]
		public Encryption.CryptoProvider Provider
		{
			get
			{
				return this.provider;
			}
			set
			{
				this.provider = value;
			}
		}
		public Encryption()
		{
			this.classLevelKey = "DevPower Encryption .NET";
		}
		public Encryption(string defaultKey) : this()
		{
			if (defaultKey.Length > 0)
			{
				this.classLevelKey = defaultKey;
			}
		}
		public Encryption(Encryption.CryptoProvider defaultProvider) : this()
		{
			this.provider = defaultProvider;
		}
		public Encryption(string defaultKey, Encryption.CryptoProvider defaultProvider) : this(defaultKey)
		{
			this.provider = defaultProvider;
		}
		public string Encrypt(string decryptedString)
		{
			return this.Encrypt(decryptedString, this.classLevelKey);
		}
		public string Decrypt(string encryptedString)
		{
			return this.Decrypt(encryptedString, this.classLevelKey);
		}
		public string Encrypt(string decryptedString, string passKey)
		{
			byte[] array = null;
			byte[] array2 = null;
			byte[] array3 = this.ConvertToByteArray(decryptedString, true);
			SymmetricAlgorithm symmetricAlgorithm = this.GetProvider(this.provider);
			if (passKey.Length == 0)
			{
				passKey = this.classLevelKey;
			}
			if (!this.makeKey(symmetricAlgorithm, passKey, ref array, ref array2))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Specified key not long enough for the ",
					this.provider.ToString(),
					" algorithm - key needs to be at least ",
					(symmetricAlgorithm.KeySize/ 8 + symmetricAlgorithm.BlockSize/ 8).ToString(),
					" characters long."
				}), "passKey");
			}
			string result;
			try
			{

                //var des = new TripleDESCryptoServiceProvider
                //{
                //    Key = array,
                //    Mode = CipherMode.CBC,
                //    IV = array2
                //};

                //var desEncrypt = des.CreateEncryptor();

                //byte[] buffer = Encoding.UTF8.GetBytes(decryptedString);
                //var ss= Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                //result = ss;

                ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateEncryptor(array, array2);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                cryptoStream.Write(array3, 0, array3.Length);
                cryptoStream.Close();
                result=this.ConvertFromByteArray(memoryStream.ToArray(), true);
            }
			catch (Exception ex)
			{
				result = "";
			}
			return result;
		}
		public string Decrypt(string encryptedString, string passKey)
		{
			byte[] array = null;
			byte[] array2 = null;
			byte[] array3 = this.ConvertToByteArray(encryptedString, false);
			SymmetricAlgorithm symmetricAlgorithm = this.GetProvider(this.provider);
			if (passKey.Length == 0)
			{
				passKey = this.classLevelKey;
			}
			if (!this.makeKey(symmetricAlgorithm, passKey, ref array, ref array2))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Specified key not long enough for the ",
					this.provider.ToString(),
					" algorithm - key needs to be at least ",
					(symmetricAlgorithm.KeySize / 8 + symmetricAlgorithm.BlockSize / 8).ToString(),
					" characters long."
				}), "passKey");
			}
			string result;
			try
			{
				ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateDecryptor(array, array2);
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
				cryptoStream.Write(array3, 0, array3.Length);
				cryptoStream.Close();
				result = this.ConvertFromByteArray(memoryStream.ToArray(), false);
			}
			catch (Exception ex)
			{
				result = "";
			}
			return result;
		}

		internal bool makeKey(SymmetricAlgorithm algorithm, string phrase, ref byte[] key, ref byte[] initVector)
		{
			int num = algorithm.KeySize / 8;
			int num2 = algorithm.BlockSize / 8;
			bool result;
			try
			{
				char[] array = phrase.ToCharArray();
				int length = array.GetLength(0);
				byte[] array2 = new byte[checked((uint)length)];
				for (int i = 0; i < length; i++)
				{
					array2[i] = (byte)array[i];
				}
				checked
				{
					key = new byte[(uint)num];
					initVector = new byte[(uint)num2];
					SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
					sHA1CryptoServiceProvider.TransformBlock(array2, 0, num, key, 0);
					sHA1CryptoServiceProvider.TransformBlock(array2, num, num2, initVector, 0);
					result = true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				result = false;
			}
			return result;
		}
		internal byte[] ConvertToByteArray(string safeString, bool encrypting)
		{
			int num = 0;
			int num2 = 0;
			string text = safeString;
			for (int i = 1; i < this.replaceStrings.Length; i++)
			{
				text = text.Replace(this.replaceStrings[i], this.searchChars[i].ToString());
			}
			int num3 = text.IndexOf(this.replaceStrings[0]);
			char[] array;
			if (num3 > -1)
			{
				while (num3 > -1 && !encrypting)
				{
					num++;
					num3 = text.IndexOf(this.replaceStrings[0], num3 + 1);
				}
				array = new char[checked((uint)unchecked(text.Length - 2 * num))];
				num3 = text.IndexOf(this.replaceStrings[0]);
				for (int j = 0; j < text.Length; j++)
				{
					if (num3 == j)
					{
						array[j] = '\0';
						j += 2;
						num3 = text.IndexOf(this.replaceStrings[0], num3 + 1);
						num2++;
					}
					else
					{
						array[j - 2 * num2] = Convert.ToChar(text.Substring(j, 1));
					}
				}
			}
			else
			{
				array = text.ToCharArray();
			}
			byte[] array2 = new byte[checked((uint)array.Length)];
			for (int k = 0; k < array2.Length; k++)
			{
				array2[k] = (byte)array[k];
			}
			return array2;
		}
		internal string ConvertFromByteArray(byte[] by, bool encrypting)
		{
			int num = by.Length;
			if (encrypting)
			{
				for (int i = 0; i < by.Length; i++)
				{
					if (by[i] == 0 || by[i] == 10 || by[i] == 13)
					{
						num += 2;
					}
				}
			}
			char[] array = new char[checked((uint)num)];
			int num2 = 0;
			for (int j = 0; j < by.Length; j++)
			{
				char c = (char)by[j];
				int num3 = this.ReplaceIndex(c);
				if (num3 == -1)
				{
					array[j + num2] = c;
				}
				else
				{
					string text = this.replaceStrings[num3];
					for (int k = 0; k < text.Length; k++)
					{
						array[j + num2 + k] = text.ToCharArray()[k];
					}
					num2 += text.Length - 1;
				}
			}
			return new string(array);
		}
		internal int ReplaceIndex(char current)
		{
			for (int i = 0; i < this.searchChars.Length; i++)
			{
				if (current == this.searchChars[i])
				{
					return i;
				}
			}
			return -1;
		}
		internal SymmetricAlgorithm GetProvider(Encryption.CryptoProvider provider)
		{
			switch (provider)
			{
			case Encryption.CryptoProvider.DES:
				return new DESCryptoServiceProvider();
			case Encryption.CryptoProvider.RC2:
				return new RC2CryptoServiceProvider();
			case Encryption.CryptoProvider.TripleDES:
				return new TripleDESCryptoServiceProvider();
			default:
				return null;
			}
		}
	}
}
