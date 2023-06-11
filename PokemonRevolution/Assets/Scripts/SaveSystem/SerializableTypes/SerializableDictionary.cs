using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> _keys = new List<TKey>();
    [SerializeField] private List<TValue> _values = new List<TValue>();

    public static SerializableDictionary<TKey, TValue> FromDictionary(Dictionary<TKey, TValue> dict)
    {
        SerializableDictionary<TKey, TValue> serializableDictionary = new SerializableDictionary<TKey, TValue>();
        foreach (KeyValuePair<TKey, TValue> pair in dict)
        {
            serializableDictionary.Add(pair.Key, pair.Value);
        }
        return serializableDictionary;
    }

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            _keys.Add(pair.Key);
            _values.Add(pair.Value);
        }
    }
    
    public void OnAfterDeserialize()
    {
        Clear();
        if (_keys.Count != _values.Count)
        {
            Debug.LogError("Keys and values have different sizes");
        }
        for (int i = 0; i < _keys.Count; i++)
        {
            Add(_keys[i], _values[i]);
        }
    }
}
