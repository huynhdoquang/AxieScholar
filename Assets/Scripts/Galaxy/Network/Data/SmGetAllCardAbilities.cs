using Net.HungryBug.Core.Network.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class SmCardAbilities
{
    public string id { get; set; }
    public string partName { get; set; }
    public string skillName { get; set; }
    public int defaultAttack { get; set; }
    public int defaultDefense { get; set; }
    public int defaultEnergy { get; set; }
    public string expectType { get; set; }
    public string iconId { get; set; }
    public string triggerColor { get; set; }
    public string triggerText { get; set; }
    public string description { get; set; }
}

[System.Serializable]
public class SmGetAllCardAbilities : IResponseData
{
    /// <summary>
    /// dict all card abilities
    /// <id, data></id>
    /// </summary>
    public Dictionary<string, SmCardAbilities> CardAbilitiesDict { get ; set; }

    public void Deserialize(byte[] data)
    {
        throw new System.NotImplementedException();
    }

    public string ToJson()
    {
        throw new System.NotImplementedException();
    }
}
