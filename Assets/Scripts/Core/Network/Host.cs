namespace Net.HungryBug.Core.Network
{
    public class Host
    {
        /// <summary>
        /// Gets the api.
        /// </summary>
        public string Api { get; }

        /// <summary>
        /// Gets the cdn.
        /// </summary>
        public string Cdn { get; }

        /// <summary>
        /// Gets the skymavis api.
        /// </summary>
        public string SkyMavisApi { get; set; }

        public string CoingeckoApi { get; set; }

        /// <summary>
        /// Create a <see cref="Host"/> from api and cdn urls.
        /// </summary>
        public Host(string api, string cdn, string skymavisApi, string coingeckoApi)
        {
            this.Api = api;
            this.Cdn = cdn;
            this.SkyMavisApi = skymavisApi;
            this.CoingeckoApi = coingeckoApi;
        }
    }
}
