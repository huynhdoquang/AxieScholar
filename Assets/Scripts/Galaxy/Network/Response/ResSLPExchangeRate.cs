using Net.HungryBug.Core.Network;
using Net.HungryBug.Galaxy.Network;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResSLPExchangeRate : GalaxyResponse<SmSLPExchangeRate>
{
    public ResSLPExchangeRate(NetResponse res) : base(res) { }

    //todo: need parse data
    protected override SmSLPExchangeRate Deserialize(byte[] data) { return new SmSLPExchangeRate(); }

    protected override SmSLPExchangeRate FromJson(string json)
    {
        var res = JsonConvert.DeserializeObject<SmSLPExchangeRate>(json);
        return res;
    }
}
