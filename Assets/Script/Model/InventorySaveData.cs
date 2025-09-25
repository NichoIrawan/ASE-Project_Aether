using UnityEngine;

[System.Serializable]
public class InventorySaveData
{
    public string itemId;
    public int slotIndex;

    public InventorySaveData(string itemId, int slotIndex)
    {
        this.itemId = itemId;
        this.slotIndex = slotIndex;
    }
}
