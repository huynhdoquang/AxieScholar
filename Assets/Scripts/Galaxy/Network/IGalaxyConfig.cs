using Net.HungryBug.Core.Network;

namespace Net.HungryBug.Galaxy.Network
{
    public interface IGalaxyConfig : IConfig
    {
        /// <summary>
        /// Allows to retrieve some data of all body parts.
        /// </summary>
        string ApiBodyPart { get; }

        /// <summary>
        /// Allows to retrieve data on specific Axie.\
        /// GET /axies/<ID>
        /// </summary>
        string ApiSingleAxie { get; }

        /// <summary>
        /// Allows to retrieve data on all exisiting Axies. Returns up to 12 Axies. Can be filtered with various parameters.
        /// </summary>
        string ApiAllAxies { get; }

        /// <summary>
        /// To retrieve Axies from specific ETH Address:
        /// /addresses/{0}/axies
        /// </summary>
        string ApiAxiesFromSpecificETHAddress { get; }

        /// <summary>
        /// get all card abilities
        /// </summary>
        string ApiGetCardAbilities { get; }

        /// <summary>
        /// Get Card Img by card id
        /// /game/cards/base/{0}.png
        /// </summary>
        string ApiGetCardImage { get; }


        /// <summary>
        /// get inventory by eth adress
        /// /clients/{0}/items
        /// </summary>
        string ApiGetInventory { get; }


        string ApiGetSLPExchangeCost { get; }
    }
}
