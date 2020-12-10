using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

public interface IPlayerprefsHelper
{
    void SaveListETHAdress(List<string> list);
    List<string> LoadListETHAdress();


    List<string> ImportFromFile(string s);
    string ExportFile(List<string> lst);
}

public class PlayerprefsHelper : IPlayerprefsHelper
{

    const string eth_adress_list_key = "eth_adress_list";

    public void SaveListETHAdress(List<string> list)
    {
        var s = JsonConvert.SerializeObject(list);
        PlayerPrefs.SetString(eth_adress_list_key, s);
    }

    public List<string> LoadListETHAdress()
    {
        var s = PlayerPrefs.GetString(eth_adress_list_key, string.Empty);
        var l = JsonConvert.DeserializeObject<List<string>>(s);

        return l;
    }

    public List<string> ImportFromFile(string s)
    {
        List<string> lst = new List<string>();
        var splits = s.Split(
            new[] { Environment.NewLine },
            StringSplitOptions.None
        );
        foreach (var item in splits)
        {
            var finalText = item.Replace(Environment.NewLine, "");
            string replacement = Regex.Replace(finalText, @"\t|\n|\r", "");
            lst.Add(replacement.Trim());
        }

        return lst;
    }

    public string ExportFile(List<string> lst)
    {
        var s = string.Empty;
        for (int i = 0; i < lst.Count; i++)
        {
            s += lst[i];
            if(i != lst.Count - 1)
            {
                s += Environment.NewLine;
            }
        }
        return s;
    }
}
