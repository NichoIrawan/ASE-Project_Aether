using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 position;
    public List<InventorySaveData> inventory;

    public GameData()
    {
        position = new Vector3(-0.5f, 4.3f, 0f);
        inventory = new List<InventorySaveData>();
    }
}
