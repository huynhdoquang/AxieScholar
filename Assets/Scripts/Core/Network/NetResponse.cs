using UnityEngine;

namespace Net.HungryBug.Core.Network
{
    [System.Serializable]
    public class NetResponse
    {
        [SerializeField]
        private int code;
        public int Code { get { return this.code; } }

        [SerializeField]
        private string message;
        public string Message { get { return this.message; } }

        [SerializeField]
        private byte[] data;
        public byte[] Data { get { return this.data; } }

        [SerializeField]
        private double time;
        public double Time { get { return this.time; } }

        [SerializeField]
        private string dataString;
        public string DataString { get { return this.dataString; } }

        public NetResponse(int code, string message, byte[] data, double time, string dataString)
        {
            this.code = code;
            this.message = message;
            this.data = data;
            this.time = time;
            this.dataString = dataString;
        }
    }
}
