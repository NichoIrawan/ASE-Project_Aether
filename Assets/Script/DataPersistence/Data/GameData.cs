using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string currentSceneName;
    public Vector3 playerPosition;
    public List<InventorySaveData> inventory;
    public SerilizableDictionary<string, EnemyData> enemies;
    public SerilizableDictionary<string, bool> collectedItem;
    public SerilizableDictionary<string, bool> collectedCollectibles;
    public SerilizableDictionary<string, bool> interactedItem;

    public GameData()
    {
        currentSceneName = "LevelScene";
        playerPosition = new Vector3(0f, 0f, 0f);
        enemies = new SerilizableDictionary<string, EnemyData>();
        inventory = new List<InventorySaveData>();
        collectedItem = new SerilizableDictionary<string, bool>();
        collectedCollectibles = new SerilizableDictionary<string, bool>();
        interactedItem = new SerilizableDictionary<string, bool>();
    }
}
