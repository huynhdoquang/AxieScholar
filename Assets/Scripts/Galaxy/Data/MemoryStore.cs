using System;
using System.Collections.Generic;

namespace Net.HungryBug.Galaxy.Data
{
    public interface IMemoryStore
    {
        /// <summary>
        /// reset some properties when login changed.
        /// </summary>
        void ResetOnChangeUser();

        SmGetAllCardAbilities SmGetAllCardAbilities { get; set; }

        SmAllBodyPart SmAllBodyPart { get; set; }

        double SLP_ExchangePrice_ETH { get; set; }

        double SLP_ExchangePrice_USD { get; set; }
    }

    public class MemoryStore : IMemoryStore
    {
        public SmGetAllCardAbilities SmGetAllCardAbilities { get; set; }
        public SmAllBodyPart SmAllBodyPart { get; set; }
        public double SLP_ExchangePrice_ETH { get ; set ; }
        public double SLP_ExchangePrice_USD { get; set; }

        //api
        public void ResetOnChangeUser()
        {
            
        }
    }
}
