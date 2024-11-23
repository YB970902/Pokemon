using System.Collections.Generic;
using UnityEngine;

public class Locale : MonoSingleton<Locale>
{
    private Dictionary<string, string> localeDict;

    protected override void Init()
    {
        base.Init();
        localeDict = new Dictionary<string, string>();
        
        TextAsset textAsset = Resources.Load<TextAsset>("Locale/Locale");
        if (textAsset.text.IsNullOrEmpty()) return;

        string[] text = textAsset.text.Split(";");
        foreach (var str in text)
        {
            string[] kv = str.Split("\"=\"");
            
            if (kv.IsNullOrEmpty()) continue;
            if (kv.Length == 1) continue;
            
            string key = kv[0].Substring(kv[0].IndexOf("\"") + 1);
            string value = kv[1].Substring(0, kv[1].LastIndexOf("\""));
            localeDict.Add(key, value);
        }
    }

    public string Localize(string _key)
    {
        if (_key.IsNullOrEmpty())
        {
            Debug.LogWarning($"해당 키는 null 입니다. \"{_key}\"");
            return _key;
        }

        if (localeDict.ContainsKey(_key) == false) return _key;
        
        return localeDict[_key];
    }

    public string Localize(string _key, params object[] _strings)
    {
        if (_strings.IsNullOrEmpty())
        {
            Debug.LogWarning($"파라미터가 없습니다.");
            return string.Empty;
        }

        if (_key.IsNullOrEmpty())
        {
            Debug.LogWarning($"해당 키는 null 입니다. \"{_key}\"");
            return _key;
        }

        return string.Format(localeDict[_key], _strings);
    }
}
