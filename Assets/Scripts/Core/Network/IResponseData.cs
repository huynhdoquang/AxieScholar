namespace Net.HungryBug.Core.Network.Data
{
    public interface IEntityData : IRequestData, IResponseData
    {

    }

    public interface IRequestData
    {
        /// <summary>
        /// Serialize <see cref="IRequestData"/> to <see cref="byte[]"/>.
        /// </summary>
        byte[] Serialize();

        /// <summary>
        /// Convert to json string.
        /// </summary>
        string ToJson();
    }

    public interface IResponseData
    {
        /// <summary>
        /// Serialize <see cref="IResponseData"/> to <see cref="byte[]"/>.
        /// </summary>
        void Deserialize(byte[] data);

        /// <summary>
        /// Convert to json string.
        /// </summary>
        string ToJson();
    }

    public interface IUniqueResponseData<TKey> : IResponseData
    {
        TKey GetKey();
    }
}
