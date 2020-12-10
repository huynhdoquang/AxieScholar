using Net.HungryBug.Core.Network;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net.HungryBug.Galaxy.Network
{
    public class ResGetAllCardAbilities : GalaxyResponse<SmGetAllCardAbilities>
    {
        public ResGetAllCardAbilities(NetResponse res) : base(res) { }

        protected override SmGetAllCardAbilities Deserialize(byte[] data) { return new SmGetAllCardAbilities(); }

        protected override SmGetAllCardAbilities FromJson(string json)
        {
            var objResponse1 = JsonConvert.DeserializeObject<Dictionary<string, SmCardAbilities>>(json);
            var res = new SmGetAllCardAbilities();
            res.CardAbilitiesDict = objResponse1;

            return res;
        }
    }
}

