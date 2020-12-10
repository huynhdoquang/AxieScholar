namespace Net.HungryBug.Core.Network
{
    public enum ResultCode
    {
        OK = 0,
    }

    public abstract class CoreResponse<T> : Response<T>
    {
        public ResultCode ResultCode { get; }

        public CoreResponse(NetResponse netResponse) : base(netResponse)
        {
            this.ResultCode = (ResultCode)netResponse.Code;
        }
    }

}
