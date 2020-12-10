using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Net.HungryBug.Core.Security
{
	/// <summary>
	/// Utilities for cipher message
	/// </summary>
	public static class Cipher
	{
		/// <summary>
		/// Generate key
		/// </summary>
		/// <returns>Base 64 key with 16 bytes length</returns>
		public static byte[] GenerateKey()
		{
			using (var rijAlg = new RijndaelManaged())
			{
				rijAlg.KeySize = 128;
				rijAlg.GenerateKey();
				return rijAlg.Key;
			}
		}

		/// <summary>
		/// Generate IV
		/// </summary>
		/// <returns>IV with 16 bytes length</returns>
		public static byte[] GenerateIv()
		{
			using (var rijAlg = new RijndaelManaged())
			{
				rijAlg.GenerateIV();
				return rijAlg.IV;
			}
		}

		private static byte[] Checksum(byte[] data)
		{
			SHA1 shaM = new SHA1Managed();
			return shaM.ComputeHash(data);
		}

		/// <summary>
		/// Encrypt data with key and iv
		/// </summary>
		/// <param name="data">Data to encrypt</param>
		/// <param name="key">Cipher key</param>
		/// <param name="iv">Cipher iv</param>
		/// <returns>Bytes encrypted data</returns>
		public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
		{
			// Check arguments.
			if (data == null)
				return null;
			if (data.Length <= 0)
				return new byte[0];
			if (key == null || key.Length != 16)
				throw new ArgumentException("key");
			if (iv == null || iv.Length != 16)
				throw new ArgumentException("iv");
			byte[] encrypted;
			var checksumData = Checksum(data);
			var inputData = new byte[checksumData.Length + data.Length];
			Buffer.BlockCopy(checksumData, 0, inputData, 0, checksumData.Length);
			Buffer.BlockCopy(data, 0, inputData, checksumData.Length, data.Length);
			using (var rijAlg = new AesManaged())
			{
				// Config parameters
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.KeySize = 128;
				rijAlg.Padding = PaddingMode.PKCS7;
				// Create an encryptor to perform the stream transform.
				var encryptor = rijAlg.CreateEncryptor(key, iv);

				// Create the streams used for encryption.
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (var swEncrypt = new BinaryWriter(csEncrypt))
						{
							//Write all data to the stream.
							swEncrypt.Write(inputData);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}
			
			return encrypted;
		}

		/// <summary>
		/// Decrypt data with key and iv
		/// </summary>
		/// <param name="data">Bytes encrypted data</param>
		/// <param name="key">Cipher key</param>
		/// <param name="iv">Cipher iv</param>
		/// <returns>Original data</returns>
		public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
		{
			// Check arguments.
			if (data == null)
				return null;
			if (data.Length <= 0)
				return new byte[0];
			if (key == null || key.Length != 16)
				throw new ArgumentException("key");
			if (iv == null || iv.Length != 16)
				throw new ArgumentException("iv");
			// Declare the string used to hold
			// the decrypted text.
			byte[] outputData;
			var encrypted = data;
			// Create an RijndaelManaged object
			// with the specified key and IV.
			using (var rijAlg = new AesManaged())
			{
				// Config parameters
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.KeySize = 128;
				rijAlg.Padding = PaddingMode.PKCS7;
				// Create a decryptor to perform the stream transform.
				var decryptor = rijAlg.CreateDecryptor(key, iv);

				// Create the streams used for decryption.
				using (var msDecrypt = new MemoryStream(encrypted))
				{
					using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (var srDecrypt = new BinaryReader(csDecrypt))
						{
							// Read the decrypted bytes from the decrypting stream
							// and place them in a string.
							outputData = srDecrypt.ReadBytes(data.Length);
						}
					}
				}
			}
			var checksumData = new byte[20];
			var originalData = new byte[outputData.Length - checksumData.Length];
			Buffer.BlockCopy(outputData, 0, checksumData, 0, checksumData.Length);
			Buffer.BlockCopy(outputData, checksumData.Length, originalData, 0, originalData.Length);
			var actualChecksumData = Checksum(originalData);
			if (checksumData.Length != actualChecksumData.Length)
			{
				throw new CryptographicException("Verify package fail");
			}
			if (!checksumData.SequenceEqual(actualChecksumData))
			{
				throw new CryptographicException("Verify package fail");
			}
			return originalData;
		}
	}
}
