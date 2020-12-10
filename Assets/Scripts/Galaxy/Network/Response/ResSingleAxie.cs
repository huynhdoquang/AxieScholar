using Net.HungryBug.Core.Network;
using Net.HungryBug.Galaxy.Network;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net.HungryBug.Galaxy.Network
{
    public class ResSingleAxie : GalaxyResponse<SmSingleAxie>
    {
        public ResSingleAxie(NetResponse res) : base(res) { }

        //todo: need parse data
        protected override SmSingleAxie Deserialize(byte[] data) { return new SmSingleAxie(); }

        protected override SmSingleAxie FromJson(string json)
        {
            var res = JsonConvert.DeserializeObject<SmSingleAxie>(json);
            return res;
        }
    }
}
