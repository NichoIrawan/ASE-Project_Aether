using UnityEngine;

[System.Serializable]
public class InventorySaveData
{
    public int itemId;
    public int slotIndex;

    public InventorySaveData(int itemId, int slotIndex)
    {
        this.itemId = itemId;
        this.slotIndex = slotIndex;
    }
}
