using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    private ItemDictionary itemDictionary;

    private void Awake()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
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
                    }
                }
            }
        }
        else if (collision.CompareTag("Collectibles"))
        {
            Collectibles collectibles = collision.GetComponent<Collectibles>();
            if (collectibles != null)
            {
                collectibles.PickUp();
                collectibles.gameObject.SetActive(false);
            }
        }
    }
}
