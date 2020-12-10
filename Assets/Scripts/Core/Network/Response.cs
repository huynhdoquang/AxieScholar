namespace Net.HungryBug.Core.Network
{
    public abstract class Response<T>
    {
        /// <summary>
        /// Gets the <see cref="NetResponse.code"/> as <see cref="ExpectedErrorCode"/>.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the value indicating whenever the response was successed.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the <see cref="NetResponse.Data"/> as <see cref="T"/>.
        /// </summary>
        public T Object { get; }

        /// <summary>
        /// Gets the raw data of <see cref="Object"/>.
        /// </summary>
        public byte[] ObjectRawData { get; }

        /// <summary>
        /// Gets current server time, base on the lastest response.
        /// </summary>
        public double ServerTime { get; }

        /// <summary>
        /// Construct the <see cref="Response{T}"/> using a <see cref="NetResponse"/>.
        /// </summary>
        /// <param name="res"></param>
        public Response(NetResponse res)
        {
            this.Code = res.Code;
            this.IsSuccess = this.Code == 0;
            this.Message = res.Message;
            this.ObjectRawData = res.Data;
            this.ServerTime = res.Time;

            if (this.IsSuccess && res.Data != null)
                this.Object = this.Deserialize(res.Data);

            if (this.IsSuccess && string.IsNullOrEmpty(res.DataString) == false)
                this.Object = this.FromJson(res.DataString);
        }

        /// <summary>
        /// Deserialize <see cref="NetResponse.Data"/> to <see cref="T"/>.
        /// </summary>
        protected abstract T Deserialize(byte[] data);

        protected abstract T FromJson(string json);
    }
}
