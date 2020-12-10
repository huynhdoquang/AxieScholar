using Net.HungryBug.Core.Network;

namespace Net.HungryBug.Galaxy.Network
{

    public class GalaxyConfig : IGalaxyConfig
    {
        private Host currentHost;

        /// <summary>
        /// The current active host.
        /// </summary>
        public Host CurrentHost
        {
            get { return this.currentHost; }
            set
            {
                this.currentHost = value;
                this.UpdateApis();
            }
        }

        public string ApiBodyPart { get; private set; }

        public string ApiSingleAxie { get; private set; }

        public string ApiAllAxies { get; private set; }

        public string ApiAxiesFromSpecificETHAddress { get; private set; }

        //
        public string ApiGetCardAbilities { get; private set; }
        public string ApiGetCardImage { get; private set; }

        //
        public string ApiGetInventory { get; private set; }

        public string ApiGetSLPExchangeCost { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        public GalaxyConfig(Host host)
        {
            this.CurrentHost = host;
        }

        /// <summary>
        /// Update apis.
        /// </summary>
        private void UpdateApis()
        {
            //
            this.ApiBodyPart = $"{this.currentHost.Api}/body-parts";
            this.ApiSingleAxie = this.currentHost.Api + "/axies/{0}";
            this.ApiAllAxies = $"{this.currentHost.Api}/axies";
            this.ApiAxiesFromSpecificETHAddress = this.currentHost.Api + "/addresses/{0}/axies";
            
            //
            this.ApiGetCardAbilities = $"{this.currentHost.Cdn}/game/cards/card-abilities.json";
            this.ApiGetCardImage = this.currentHost.Cdn + "/game/cards/base/{0}.png";

            //
            this.ApiGetInventory = this.currentHost.SkyMavisApi + "/clients/{0}/items";

            //
            this.ApiGetSLPExchangeCost = this.currentHost.CoingeckoApi + "/simple/price?ids=small-love-potion&vs_currencies=eth%2Cusd";
        }
    }
}
