using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    [SerializeField]private Dictionary<string, GameObject> itemDictionary = new();

    public static ItemDictionary Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one instance of ItemDictionary found. Destroying the new one");
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // Auto Increment
        for (int i = 0; i< itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].id = (i + 1).ToString();
            }
        }

        foreach (Item item in itemPrefabs)
        {
            itemDictionary[item.id] = item.gameObject;
        }
    }

    public GameObject getItemPrefab(string id)
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
