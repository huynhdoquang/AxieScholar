namespace Net.HungryBug.Core.Network
{
    /// <summary>
    /// Network tranfering model interface
    /// </summary>
    public interface INetModel
    {
        /// <summary>
        /// Deserialize from data bytes to object
        /// </summary>
        void Deserialize(byte[] data);

        /// <summary>
        /// Serialize from object to data bytes
        /// </summary>
        byte[] Serialize();

        /// <summary>
        /// Serialize json String
        /// </summary>
        /// <returns></returns>
        string ToJson();
    }

    public class NetMessage
    {
        public int ResultCode { get; }
        public INetModel Model { get; }

        public NetMessage(int resultCode, INetModel model)
        {
            this.ResultCode = resultCode;
            this.Model = model;
        }
    }
}
