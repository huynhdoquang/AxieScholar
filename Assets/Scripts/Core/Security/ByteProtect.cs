﻿namespace Net.HungryBug.Core.Security
{
    public class ByteProtect
    {
        private static readonly byte[] EncryptMap = new byte[] { 52, 70, 196, 201, 25, 211, 251, 38, 253, 193, 182, 33, 229, 43, 120, 81, 183, 18, 213, 22, 98, 106, 95,
            231, 4, 199, 129, 166, 71, 200, 6, 57, 132, 153, 76, 202, 82, 244, 247, 221, 186, 108, 20, 30, 21, 170, 37, 73, 90, 72, 164, 54, 214, 103, 36, 56, 149,
            243, 138, 233, 203, 240, 28, 61, 24, 96, 157, 210, 99, 185, 234, 218, 14, 104, 239, 188, 192, 111, 163, 13, 178, 46, 34, 137, 102, 230, 147, 0, 27, 250,
            194, 62, 11,222, 195, 48, 94, 112, 55, 146, 10, 169, 135, 206, 12, 177, 1, 29, 3, 115, 155, 140, 105, 171, 219, 217, 223, 180, 59, 172, 212, 191, 121, 136,
            145, 83, 248, 79, 8, 204, 110, 228, 26, 50, 187, 16, 252, 152, 97, 74, 117, 58, 2, 107,119, 154, 41, 227, 208, 220, 179, 225, 125, 235, 159, 141, 128, 63,
            254, 168, 197, 246, 5, 174, 31, 122, 198, 184, 161, 237, 51, 114, 150, 226, 65, 66, 127, 39, 176, 100, 75, 44, 167, 17, 139, 19, 35, 242, 156, 78, 92, 89,
            207, 91, 116, 143, 224, 123, 23, 162, 118, 64, 175, 236, 53, 249, 45, 181, 93, 142, 77, 144, 190, 158, 131, 101, 88, 87, 47, 209, 124, 205, 40, 238, 86, 32,
            49, 126, 80, 68, 67, 84, 130, 109, 165, 9, 173, 245, 85, 113, 215, 255, 134, 160, 133, 69, 60, 241, 151, 7, 42, 189, 216, 148, 232, 15 };


        private static readonly byte[] DecryptMap = new byte[] { 87, 106, 142, 108, 24, 162, 30, 249, 128, 235, 100, 92, 104, 79, 72, 255, 135, 183, 17, 185, 42, 44, 19,
            198, 64, 4, 132, 88, 62, 107, 43, 164, 225, 11, 82, 186, 54, 46, 7, 177, 222, 146, 250, 13, 181, 206, 81, 218, 95, 226, 133, 170, 0, 204, 51, 98, 55, 31,
            141, 118, 246, 63, 91, 157, 201, 174, 175, 230, 229, 245, 1, 28, 49, 47, 139, 180, 34, 210, 189, 127, 228, 15, 36, 125, 231, 238, 224, 217, 216, 191, 48,
            193, 190, 208, 96, 22, 65, 138, 20, 68, 179, 215, 84, 53, 73, 112, 21, 143, 41, 233, 130, 77, 97, 239, 171, 109, 194, 140, 200, 144, 14, 122, 165, 197, 220,
            152, 227, 176, 156, 26, 232, 214, 32, 244, 242, 102, 123, 83, 58, 184, 111, 155, 209, 195, 211, 124, 99, 86, 253, 56, 172, 248, 137, 33, 145, 110, 188, 66,
            213, 154, 243, 168, 199, 78, 50, 234, 27, 182, 159, 101, 45, 113, 119, 236, 163, 202, 178, 105, 80, 150, 117, 207, 10, 16, 167, 69, 40, 134, 75, 251, 212,
            121, 76, 9, 90, 94, 2, 160, 166, 25, 29, 3, 35, 60, 129, 221, 103, 192, 148, 219, 67, 5, 120, 18, 52, 240, 252, 115, 71, 114, 149, 39, 93, 116, 196, 151, 173,
            147, 131, 12, 85, 23, 254, 59, 70, 153, 203, 169, 223, 74, 61, 247, 187, 57, 37, 237, 161, 38, 126, 205, 89, 6, 136, 8, 158, 241 };

        public static byte[] EncryptBytes(byte[] inputBytes)
        {
            var byteResult = new byte[inputBytes.Length];
            for (var i = 0; i < inputBytes.Length; i++)
            {
                byteResult[i] = EncryptMap[inputBytes[i]];
            }

            return byteResult;
        }

        public static byte[] DecryptBytes(byte[] inputBytes)
        {
            var byteResult = new byte[inputBytes.Length];
            for (var i = 0; i < inputBytes.Length; i++)
            {
                byteResult[i] = DecryptMap[inputBytes[i]];
            }

            return byteResult;
        }
    }
}