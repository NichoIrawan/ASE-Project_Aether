using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour, IDataPersistence
{
    private InventoryController inventoryController;
    private ItemDictionary itemDictionary;
    private List<int> collectedItemIds;

    private void Awake()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        collectedItemIds = new List<int>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                GameObject itemPrefab = itemDictionary.getItemPrefab(item.id);

                if (itemPrefab != null)
                {
                    bool itemAdded = inventoryController.AddInventoryItem(itemPrefab);

                    if (itemAdded)
                    {   
                        item.PickUp();
                        item.gameObject.SetActive(false);
                        collectedItemIds.Add(item.id);
                    }
                }

            }
        }
    }

    void IDataPersistence.LoadData(GameData data)
    {
        //collectedItemIds = data.collectedItemId;
    }

    void IDataPersistence.SaveData(ref GameData data)
    {
        //data.collectedItemId = collectedItemIds;
    }
}
