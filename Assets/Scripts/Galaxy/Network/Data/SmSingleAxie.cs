using Net.HungryBug.Core.Network.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class Effect
{
    public string name { get; set; }
    public string type { get; set; }
    public string description { get; set; }
}

public class Move
{
    public string id { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public int attack { get; set; }
    public int defense { get; set; }
    public int accuracy { get; set; }
    public int stage { get; set; }
    public List<Effect> effects { get; set; }
}

public class Part
{
    public string id { get; set; }
    public string name { get; set; }
    public string @class { get; set; }
    public string type { get; set; }
    public bool mystic { get; set; }
    public bool bionic { get; set; }
    public bool xmas { get; set; }
    public int stage { get; set; }
    public List<Move> moves { get; set; }
}

public class Images
{
    //[JsonProperty("axie.png")]
    public string AxiePng { get; set; }
}

public class Figure
{
    public Images images { get; set; }
    public string atlas { get; set; }
    public string model { get; set; }
}

public class Auction
{
    public string type { get; set; }
    public string startingPrice { get; set; }
    public string endingPrice { get; set; }
    public string buyNowPrice { get; set; }
    public string suggestedPrice { get; set; }
    public string startingTime { get; set; }
    public string duration { get; set; }
    public string timeLeft { get; set; }
}

public class Stats
{
    public int hp { get; set; }
    public int speed { get; set; }
    public int skill { get; set; }
    public int morale { get; set; }
}

[System.Serializable]
public class SmSingleAxie : IResponseData
{
    public int id { get; set; }
    public string name { get; set; }
    public string genes { get; set; }
    public string owner { get; set; }
    public int birthDate { get; set; }
    public int sireId { get; set; }
    public string sireClass { get; set; }
    public int matronId { get; set; }
    public string matronClass { get; set; }
    public int stage { get; set; }
    public string @class { get; set; }
    public object title { get; set; }
    public List<Part> parts { get; set; }
    public string image { get; set; }
    public Figure figure { get; set; }
    public Auction auction { get; set; }
    public Stats stats { get; set; }
    public int exp { get; set; }
    public int activityPoint { get; set; }
    public int pendingExp { get; set; }
    public long expSubmittedAt { get; set; }
    public string expSignature { get; set; }
    public int breedCount { get; set; }
    public bool breedable { get; set; }
    public int level { get; set; }
    public bool unlocked { get; set; }

    public void Deserialize(byte[] data)
    {
        throw new System.NotImplementedException();
    }

    public string ToJson()
    {
        throw new System.NotImplementedException();
    }
}

