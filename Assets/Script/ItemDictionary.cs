using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();

        // Auto Increment
        for (int i = 0; i< itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].id = i + 1;
            }
        }

        foreach (Item item in itemPrefabs)
        {
            itemDictionary[item.id] = item.gameObject;
        }
    }

    public GameObject getItemPrefab(int id)
    {
        // Search item in dictionary
        itemDictionary.TryGetValue(id, out GameObject prefab);

        // Handling if not found
        if (prefab == null)
        {
            Debug.LogWarning($"Item with ID {id} not found");
        }
        return prefab;
    }
}
