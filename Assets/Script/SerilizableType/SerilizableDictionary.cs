using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerilizableDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<Tkey> keys = new List<Tkey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        { 
            Debug.LogError($"there are {keys.Count} keys and {values.Count} values after deserialization. " +
                $"Make sure that both key and value types are serializable."); 
        }

        for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
        {
            this.Add(keys[i], values[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<Tkey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
}
