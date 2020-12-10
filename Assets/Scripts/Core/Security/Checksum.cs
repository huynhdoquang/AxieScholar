using System.IO;
using System.Text;

using System.Security.Cryptography;

namespace Net.HungryBug.Core.Security
{
    public interface IChecksum
    {
        /// <summary>
        /// Checksum data use HMAC_SHA1
        /// </summary>
        /// <param name="secret">Secret key</param>
        /// <param name="data">Data to checksum</param>
        /// <returns>Byte array of checksum signature</returns>
        byte[] HmacSha1(byte[] secret, byte[] data);

        /// <summary>
        /// Verify signature data use HMAC_SHA1
        /// </summary>
        /// <param name="secret">Secret key</param>
        /// <param name="data">Data to checksum</param>
        /// <param name="signature">Signature to verify</param>
        /// <returns></returns>
        bool VerifyHmacSha1(byte[] secret, byte[] data, byte[] signature);

        /// <summary>
        /// Hash md5
        /// </summary>
        byte[] HashMd5(byte[] data);

        /// <summary>
        /// Hash a byte array by MD5 algorithm
        /// </summary>
        /// <param name="data">Bytes data to hash</param>
        /// <returns>Hash result as Hex string</returns>
        string HashMd5Data(byte[] data);

        /// <summary>
        /// Hash a string by MD5 algorithm
        /// </summary>
        /// <param name="data">String to hash</param>
        /// <returns>Hash result as Hex string</returns>
        string HashMd5String(string data);

        /// <summary>
        /// Hash a file data by MD5 algorithm
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Hash result as Hex string</returns>
        string HashMd5File(string path);
    }

    /// <summary>
    /// Utilities for checksum data
    /// </summary>
    public class Checksum : IChecksum
    {
        /// <summary>
        /// Checksum data use HMAC_SHA1
        /// </summary>
        /// <param name="secret">Secret key</param>
        /// <param name="data">Data to checksum</param>
        /// <returns>Byte array of checksum signature</returns>
        public byte[] HmacSha1(byte[] secret, byte[] data)
        {
            // Check arguments.
            if (data == null || secret == null || secret.Length == 0 || data.Length == 0)
                return null;
            var hash = new HMACSHA1(secret);
            return hash.ComputeHash(data);
        }

        /// <summary>
        /// Verify signature data use HMAC_SHA1
        /// </summary>
        /// <param name="secret">Secret key</param>
        /// <param name="data">Data to checksum</param>
        /// <param name="signature">Signature to verify</param>
        /// <returns></returns>
        public bool VerifyHmacSha1(byte[] secret, byte[] data, byte[] signature)
        {
            var checksum = HmacSha1(secret, data);
            if (checksum == null || signature == null) return false;
            if (checksum.Length != signature.Length) return false;
            int i;
            for (i = 0; i < checksum.Length; i++)
            {
                if (checksum[i] != signature[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Hash md5
        /// </summary>
        public byte[] HashMd5(byte[] data)
        {
            // Check arguments.
            if (data == null || data.Length == 0)
                return null;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                return md5.ComputeHash(data);
            }
        }

        /// <summary>
        /// Hash a byte array by MD5 algorithm
        /// </summary>
        /// <param name="data">Bytes data to hash</param>
        /// <returns>Hash result as Hex string</returns>
        public string HashMd5Data(byte[] data)
        {
            if (data == null || data.Length == 0)
                return null;
            return ToHexString(HashMd5(data));
        }

        /// <summary>
        /// Hash a string by MD5 algorithm
        /// </summary>
        /// <param name="data">String to hash</param>
        /// <returns>Hash result as Hex string</returns>
        public string HashMd5String(string data)
        {
            if (data == null || data.Length == 0)
                return null;
            return ToHexString(HashMd5(Encoding.UTF8.GetBytes(data)));
        }

        /// <summary>
        /// Hash a file data by MD5 algorithm
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Hash result as Hex string</returns>
        public string HashMd5File(string path)
        {
            if (!File.Exists(path)) return null;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = md5.ComputeHash(stream);
                    return ToHexString(hash);
                }
            }
        }

        /// <summary>
        /// Convert byte array to hex string representation
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ToHexString(byte[] data)
        {
            var strBuilder = new StringBuilder();
            int i;
            for (i = 0; i < data.Length; i++)
            {
                strBuilder.Append(data[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}
