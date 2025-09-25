using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public List<InventorySaveData> inventory;
    public SerilizableDictionary<string, EnemyData> enemies;
    public SerilizableDictionary<string, bool> collectedItem;
    public SerilizableDictionary<string, bool> collectedCollectibles;
    public SerilizableDictionary<string, bool> interactedItem;

    public GameData()
    {
        playerPosition = new Vector3(-0.5f, 4.3f, 0f);
        enemies = new();
        inventory = new();
        collectedItem = new();
        collectedCollectibles = new();
        interactedItem = new();
    }
}
