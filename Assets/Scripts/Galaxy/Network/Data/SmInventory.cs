using Net.HungryBug.Core.Network.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class Signature
{
    public string signature { get; set; }
    public int amount { get; set; }
    public int timestamp { get; set; }
}

public class BlockchainRelated
{
    public Signature signature { get; set; }
    public object balance { get; set; }
    public object checkpoint { get; set; }
    public object block_number { get; set; }
}

public class Item2
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string image_url { get; set; }
    public int updated_at { get; set; }
    public int created_at { get; set; }
}

public class Item
{
    public string client_id { get; set; }
    public int item_id { get; set; }
    public int total { get; set; }
    public BlockchainRelated blockchain_related { get; set; }
    public Item2 item { get; set; }
}

[System.Serializable]
public class SmInventory : IResponseData
{
    public bool success { get; set; }
    public List<Item> items { get; set; }
    public int offset { get; set; }
    public int limit { get; set; }

    public void Deserialize(byte[] data)
    {
        throw new System.NotImplementedException();
    }

    public string ToJson()
    {
        throw new System.NotImplementedException();
    }
}
