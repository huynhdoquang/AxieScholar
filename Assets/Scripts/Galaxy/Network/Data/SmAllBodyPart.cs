using Net.HungryBug.Core.Network.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class SmBodyPartInfor
{
    public string partId { get; set; }
    public string @class { get; set; }
    public string specialGenes { get; set; }
    public string type { get; set; }
    public string name { get; set; }
}


[System.Serializable]
public class SmAllBodyPart : IResponseData
{
    public List<SmBodyPartInfor> MyArray { get; set; }

    public void Deserialize(byte[] data)
    {
        throw new System.NotImplementedException();
    }

    public string ToJson()
    {
        throw new System.NotImplementedException();
    }
}
