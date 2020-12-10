using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Net.HungryBug.Core.Security
{
    public interface IRSAService
    {
        /// <summary>
        /// Create Signature from data.
        /// </summary>
        byte[] CreateSignature(byte[] data);

        /// <summary>
        /// Create signature from a string then base64 the output.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string CreateSignature(string data);

        /// <summary>
        /// 
        /// </summary>
        byte[] EncryptBytes(byte[] source);

        /// <summary>
        /// 
        /// </summary>
        byte[] DecryptBytes(byte[] source);
    }

    public class RSAService : IRSAService
    {
        /// <summary>
        /// 
        /// </summary>
        private RSACryptoServiceProvider ProvideRSACryptoServiceProvider()
        {
            var signer = new RSACryptoServiceProvider();
            signer.FromXmlString("<RSAKeyValue><Modulus>bVgjsGAQBzoWYZKkDh7K1eWmbIW5a5gf1MEDqy8fp/sHy22T+/6h6NJUxt4AFyvNkh/ilXH/uInY38W/M9E4QfXvJKKOLu4asSAgd6yVBQ06IH1V34bap0cL0l8Dwnk96Ltx5zwIe+41/wrk6GN+UwIv+YhDrxS39/mPKbxfw5c=</Modulus><Exponent>AQAB</Exponent><P>v697dHD2i7qetiUUz5eX3XYriqkSDV7as5Bm42crsgB5+PCwqkZ64zdB5bBylLKb5D/N2Enh50LuYoBT75k25w==</P><Q>kggXxMe7o3HRZA8Tf/3nbFIwf9HVTnYX8JrvNnFvdYyiu7/UFsZPzWkKoBGHq4tSp78GbHLIexZr2LEPK9Vn0Q==</Q><DP>JHzH2DtkguaMLYnTc0yc6NgEo8lZGVhz8vhKzYCnDaQPk7ZlOpRd8gBjt9Alj26MRB9tFH3D1Zyk+fUJhTM6Aw==</DP><DQ>NTAZaCTPZQKJn4YWNiIGbl+pJoUIyiEdKxdjVVMPo6LrirKdzwW//zLpGdAIuCRwoIRlFI7iEmLiOPSUcAS0YQ==</DQ><InverseQ>JB1sm/z+JKzENGqW232C7QL6PEqPS5Dmp97NSewCsjlLyOCs0dx7unbZG4jJq+3gGeAm4AJpRmnF+q4HiSR+8g==</InverseQ><D>JICF+X04HJGBozUG20fgQqOVeSjzCz4PckC07tKeaL9ejMSZ4EfS2RxN/M1jjlirGZrXUzhpO7uY0Kr891lixAGOI1bcmPZMJxzJFH1hN9BVDFWjT7FdCD5PXmzgyRF2ScX/jNatay+554yWapUOC/uA0IXIbwL0nwhL2eEkX0E=</D></RSAKeyValue>");
            return signer;
        }

        /// <summary>
        /// 
        /// </summary>
        private RSA ProvideRSA()
        {
            var signer = RSA.Create();
            signer.FromXmlString("<RSAKeyValue><Modulus>bVgjsGAQBzoWYZKkDh7K1eWmbIW5a5gf1MEDqy8fp/sHy22T+/6h6NJUxt4AFyvNkh/ilXH/uInY38W/M9E4QfXvJKKOLu4asSAgd6yVBQ06IH1V34bap0cL0l8Dwnk96Ltx5zwIe+41/wrk6GN+UwIv+YhDrxS39/mPKbxfw5c=</Modulus><Exponent>AQAB</Exponent><P>v697dHD2i7qetiUUz5eX3XYriqkSDV7as5Bm42crsgB5+PCwqkZ64zdB5bBylLKb5D/N2Enh50LuYoBT75k25w==</P><Q>kggXxMe7o3HRZA8Tf/3nbFIwf9HVTnYX8JrvNnFvdYyiu7/UFsZPzWkKoBGHq4tSp78GbHLIexZr2LEPK9Vn0Q==</Q><DP>JHzH2DtkguaMLYnTc0yc6NgEo8lZGVhz8vhKzYCnDaQPk7ZlOpRd8gBjt9Alj26MRB9tFH3D1Zyk+fUJhTM6Aw==</DP><DQ>NTAZaCTPZQKJn4YWNiIGbl+pJoUIyiEdKxdjVVMPo6LrirKdzwW//zLpGdAIuCRwoIRlFI7iEmLiOPSUcAS0YQ==</DQ><InverseQ>JB1sm/z+JKzENGqW232C7QL6PEqPS5Dmp97NSewCsjlLyOCs0dx7unbZG4jJq+3gGeAm4AJpRmnF+q4HiSR+8g==</InverseQ><D>JICF+X04HJGBozUG20fgQqOVeSjzCz4PckC07tKeaL9ejMSZ4EfS2RxN/M1jjlirGZrXUzhpO7uY0Kr891lixAGOI1bcmPZMJxzJFH1hN9BVDFWjT7FdCD5PXmzgyRF2ScX/jNatay+554yWapUOC/uA0IXIbwL0nwhL2eEkX0E=</D></RSAKeyValue>");
            return signer;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] CreateSignature(byte[] data)
        {
            var signer = this.ProvideRSACryptoServiceProvider();
            byte[] signature = signer.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return signature;
        }

        /// <summary>
        /// Create signature from a string then base64 the output.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string CreateSignature(string data)
        {
            var signInputBytes = System.Text.Encoding.ASCII.GetBytes(data);
            var signBytes = this.CreateSignature(signInputBytes);
            return Convert.ToBase64String(signBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] EncryptBytes(byte[] source)
        {
            return ByteProtect.EncryptBytes(source);
            //var signer = this.ProvideRSA();
            //return this.Encrypt(signer, source);
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] DecryptBytes(byte[] source)
        {
            return ByteProtect.DecryptBytes(source);
            //try
            //{
            //    var signer = this.ProvideRSA();
            //    return this.Decrypt(signer, source);
            //}
            //catch (Exception e)
            //{
            //    Debug.LogError(e);
            //    return new byte[] { 1 };
            //}
        }

        private byte[] Encrypt(RSA rsa, byte[] input)
        {
            // by default this will create a 128 bits AES (Rijndael) object
            SymmetricAlgorithm sa = SymmetricAlgorithm.Create();
            ICryptoTransform ct = sa.CreateEncryptor();
            byte[] encrypt = ct.TransformFinalBlock(input, 0, input.Length);

            RSAPKCS1KeyExchangeFormatter fmt = new RSAPKCS1KeyExchangeFormatter(rsa);
            byte[] keyex = fmt.CreateKeyExchange(sa.Key);

            // return the key exchange, the IV (public) and encrypted data
            byte[] result = new byte[keyex.Length + sa.IV.Length + encrypt.Length];
            Buffer.BlockCopy(keyex, 0, result, 0, keyex.Length);
            Buffer.BlockCopy(sa.IV, 0, result, keyex.Length, sa.IV.Length);
            Buffer.BlockCopy(encrypt, 0, result, keyex.Length + sa.IV.Length, encrypt.Length);
            return result;
        }

        private byte[] Decrypt(RSA rsa, byte[] input)
        {
            // by default this will create a 128 bits AES (Rijndael) object
            SymmetricAlgorithm sa = SymmetricAlgorithm.Create();

            byte[] keyex = new byte[rsa.KeySize >> 3];
            Buffer.BlockCopy(input, 0, keyex, 0, keyex.Length);

            RSAPKCS1KeyExchangeDeformatter def = new RSAPKCS1KeyExchangeDeformatter(rsa);
            byte[] key = def.DecryptKeyExchange(keyex);

            byte[] iv = new byte[sa.IV.Length];
            Buffer.BlockCopy(input, keyex.Length, iv, 0, iv.Length);

            ICryptoTransform ct = sa.CreateDecryptor(key, iv);
            byte[] decrypt = ct.TransformFinalBlock(input, keyex.Length + iv.Length, input.Length - (keyex.Length + iv.Length));
            return decrypt;
        }
    }
}
