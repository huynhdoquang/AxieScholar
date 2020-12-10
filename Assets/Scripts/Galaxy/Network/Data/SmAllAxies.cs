using Net.HungryBug.Core.Network.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SmAllAxies : IResponseData
{
    public string axies;
    public string totalAxies;

    public void Deserialize(byte[] data)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Convert from 
    /// </summary>
    public static SmContainer FromJson(string json) { return UnityEngine.JsonUtility.FromJson<SmContainer>(json); }

    public string ToJson()
    {
        throw new System.NotImplementedException();
    }
}
