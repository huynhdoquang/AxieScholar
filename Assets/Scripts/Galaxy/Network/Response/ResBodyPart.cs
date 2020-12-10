using Net.HungryBug.Core.Network;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net.HungryBug.Galaxy.Network
{
    public class ResBodyPart : GalaxyResponse<SmAllBodyPart>
    {
        public ResBodyPart(NetResponse res) : base(res) { }

        protected override SmAllBodyPart Deserialize(byte[] data) { return new SmAllBodyPart(); }

        protected override SmAllBodyPart FromJson(string json)
        {
            var objResponse1 = JsonConvert.DeserializeObject<List<SmBodyPartInfor>>(json);
            var res = new SmAllBodyPart();
            res.MyArray = objResponse1;
            return res;
        }
    }
}

