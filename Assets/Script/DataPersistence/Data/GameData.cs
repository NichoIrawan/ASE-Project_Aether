using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 position;
    public List<InventorySaveData> inventory;
    public SerilizableDictionary<int, bool> collectedItem;
    public SerilizableDictionary<string, bool> interactedItem;

    public GameData()
    {
        position = new Vector3(-0.5f, 4.3f, 0f);
        inventory = new List<InventorySaveData>();
        collectedItem = new SerilizableDictionary<int, bool>();
        interactedItem = new SerilizableDictionary<string, bool>();
    }
}
