using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<Key, TValue> : Dictionary<Key, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<Key> keys = new List<Key>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnAfterDeserialize()
    {
        this.Clear();
        if (keys.Count != values.Count)
        {
            Debug.Log("Keys count is not equal to value const");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<Key, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
}
