using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour, IDataPersistence
{
    private ItemDictionary itemDictionary;

    // Event for inventory change
    public static event Action OnInventoryChanged;

    public GameObject inventoryPanel;
    public GameObject inventoryPrefab;
    public GameObject[] items;
    public int slotCount;

    private void Awake()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();

        // Create inventory slots based on slot counts data
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(inventoryPrefab, inventoryPanel.transform);
        }
    }

    public GameObject GetItemInSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slotCount)
        {
            Slot slot = inventoryPanel.transform.GetChild(slotIndex).GetComponent<Slot>();
            Debug.Log($"current item is {slot.currentItem}");
            return slot.currentItem;
        }
        return null;
    }

    public void SwapItem(Slot slotA, Slot slotB)
    {
        // Check if both slots are valid
        if (slotA == null || slotB == null) return;

        // Temp store slot A item
        GameObject tempItem = slotA.currentItem;

        // Move slot B item to slot A
        slotA.currentItem = slotB.currentItem;
        if (slotA.currentItem != null)
        {
            slotA.currentItem.transform.SetParent(slotA.transform);
            slotA.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        // Move temp item to slot B
        slotB.currentItem = tempItem;
        if (slotB.currentItem != null)
        {
            slotB.currentItem.transform.SetParent(slotB.transform);
            slotB.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        // Notify inventory change
        OnInventoryChanged?.Invoke();
        Debug.Log("Items swapped between slots.");
    }

    public bool AddInventoryItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            // Check empty slot
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                // Initialize item prefab
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;

                // Notify inventory change
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        Debug.LogError("There are no empty slots left in inventory");
        return false;
    }

    public void LoadData(GameData data)
    {
        // Load inventory data
        List<InventorySaveData> inventorySaveDatas = data.inventory;

        // Clear existing inventory slots
        foreach (Transform child in inventoryPanel.transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Destroy(slot.currentItem);
                slot.currentItem = null;
            }
        }

        // Assign items to slots based on loaded data
        foreach (InventorySaveData invData in inventorySaveDatas)
        {
            if (invData.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(invData.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.getItemPrefab(invData.itemId);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    slot.currentItem = item;
                }
            }
        }

        // Notify inventory change
        OnInventoryChanged?.Invoke();
        Debug.Log($"Loaded {inventorySaveDatas.Count} item data.");
    }

    public void SaveData(ref GameData data)
    {
        // Create Internal Inventory Data List
        List<InventorySaveData> invData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            // Checking filled inventory slot
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                // Add item to inventory save data
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData(item.id, slot.transform.GetSiblingIndex()));
            }
        }

        data.inventory = invData;
        Debug.Log($"Saving inventory data consisting of {data.inventory.Count} items.");
    }
}
