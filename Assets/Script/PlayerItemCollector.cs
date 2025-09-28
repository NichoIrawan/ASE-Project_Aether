using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    [SerializeField]private InventoryController inventoryController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                GameObject itemPrefab = ItemDictionary.Instance.getItemPrefab(item.id);

                if (itemPrefab != null)
                {
                    bool itemAdded = inventoryController.AddInventoryItem(itemPrefab);

                    if (itemAdded)
                    {   
                        item.PickUp();
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
            }
        }
    }
}
