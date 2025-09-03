using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour, IDataPersistence
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject inventoryPrefab;
    public GameObject[] itemPrefabs;
    public int slotCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();

        //for (int i = 0; i < slotCount; i++)
        //{
        //    // Create inventory slots
        //    Slot slot = Instantiate(inventoryPrefab, inventoryPanel.transform).GetComponent<Slot>();

        //    // Assign item to slot
        //    if (i < itemPrefabs.Length)
        //    {
        //        GameObject item = Instantiate(itemPrefabs[i], slot.transform);
        //        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //        slot.currentItem = item;
        //    }
        //}
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
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = itemPrefab;

                return true;
            }
        }

        Debug.LogError("There are no empty slots left in inventory");
        return false;
    }

    void IDataPersistence.LoadData(GameData data)
    {
        // Load inventory data
        List<InventorySaveData> inventorySaveDatas = data.inventory;

        // Clear existing inventory slots
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create inventory slots based on slot counts data
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(inventoryPrefab, inventoryPanel.transform);
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

        Debug.Log($"Loaded {inventorySaveDatas.Count} item data.");
    }

    void IDataPersistence.SaveData(ref GameData data)
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
