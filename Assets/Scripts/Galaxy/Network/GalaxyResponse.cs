using Net.HungryBug.Core.Network;

namespace Net.HungryBug.Galaxy.Network
{
    public enum ResultCode
    {
        OK = 0,
    }

    public abstract class GalaxyResponse<T> : Response<T>
    {
        /// <summary>
        /// Gets the <see cref="NetResponse.Code"/> as <see cref="Network.ResultCode"/>.
        /// </summary>
        public ResultCode ResultCode { get; }

        public GalaxyResponse(NetResponse netResponse) : base(netResponse)
        {
            this.ResultCode = (ResultCode)netResponse.Code;
        }
    }
}
