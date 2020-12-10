using Net.HungryBug.Core.Network.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class PublicProfileWithEthereumAddress
{
    public string accountId { get; set; }
    public string name { get; set; }
    public string __typename { get; set; }
}

public class Data
{
    public PublicProfileWithEthereumAddress publicProfileWithEthereumAddress { get; set; }
}


[System.Serializable]
public class QmGetPublicAdress : IResponseData
{
    public Data data { get; set; }

    public void Deserialize(byte[] data)
    {
        throw new System.NotImplementedException();
    }

    public string ToJson()
    {
        throw new System.NotImplementedException();
    }
}