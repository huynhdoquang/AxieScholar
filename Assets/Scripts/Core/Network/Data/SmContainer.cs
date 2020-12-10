namespace Net.HungryBug.Core.Network.Data
{
    [System.Serializable]
    public class SmContainer : IResponseData
    {
        public int Code;
        public string Message;
        public double Time;
        public byte[] Data;

        public string DataString;

        /// <summary>
        /// Construct a new <see cref="CmGetProperty"/>.
        /// </summary>
        public SmContainer() { }
        public SmContainer(string dataString) { DataString = dataString; }


        /// <summary>
        /// Convert from <see cref="byte[]"/> to <see cref="SmContainer"/>.
        /// </summary>
        public void Deserialize(byte[] data)
        {
            /*var fbfObject = SmContainerData.GetRootAsSmContainerData(new ByteBuffer(data));
            this.FromFlatBufferObject(fbfObject);*/
        }

		/// <summary>
        /// Create <see cref="byte[]"/> from <see cref="SmContainer"/>.
        /// </summary>
        private byte[] Serialize()
        {
            /*var buffer = new FlatBufferBuilder(1);
            var offset = ToOffset(buffer);

            buffer.Finish(offset.Value);
            return buffer.DataBuffer.ToSizedArray();*/

            return null;
        }

        /// <summary>
        /// Convert <see cref="SmContainer"/> to json.
        /// </summary>
        public string ToJson() { return UnityEngine.JsonUtility.ToJson(this, true); }

        /// <summary>
        /// Convert from 
        /// </summary>
        public static SmContainer FromJson(string json) { return UnityEngine.JsonUtility.FromJson<SmContainer>(json); }
    }
}