using Net.HungryBug.Core.Network.Data;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class SmallLovePotion
{
    public double eth { get; set; }
    public double usd { get; set; }
}

[System.Serializable]
public class SmSLPExchangeRate : IResponseData
{
    [JsonProperty("small-love-potion")]
    public SmallLovePotion SmallLovePotion { get; set; }

    public void Deserialize(byte[] data)
    {
        throw new System.NotImplementedException();
    }

    public string ToJson()
    {
        throw new System.NotImplementedException();
    }
}
