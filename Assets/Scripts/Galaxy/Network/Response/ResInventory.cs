using Net.HungryBug.Core.Network;
using Net.HungryBug.Galaxy.Network;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResInventory : GalaxyResponse<SmInventory>
{
    public ResInventory(NetResponse res) : base(res) { }

    //todo: need parse data
    protected override SmInventory Deserialize(byte[] data) { return new SmInventory(); }

    protected override SmInventory FromJson(string json)
    {
        var res = JsonConvert.DeserializeObject<SmInventory>(json);
        return res;
    }
}
