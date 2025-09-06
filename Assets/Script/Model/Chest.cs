using UnityEngine;

public class Chest : MonoBehaviour, IInteractable, IDataPersistence
{
    public string ChestID;

    public bool IsOpened { get; private set; }
    public GameObject ContainedItemPrefab;
    public Sprite OpenedSprite;

    public GameObject RequiredItem; // Item required to open the chest, can be null

    [ContextMenu("Open Chest")]
    public void TestOpenChest()
    {
        Interact();
    }

    [ContextMenu("Generate guid")]
    private void GenerateGuid()
    {
        ChestID = System.Guid.NewGuid().ToString();
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (IsOpened) return;

        // Check if the player has the required item
        if (RequiredItem != null)
        {
            var equippedItem = HotbarController.EquippedItem ? HotbarController.EquippedItem.GetComponent<Item>() : null;
            var requiredItem = RequiredItem ? RequiredItem.GetComponent<Item>() : null;

            // If player has no item equipped
            if (!equippedItem || !requiredItem) 
            { 
                Debug.Log("You need a " + requiredItem.itemName + " to open this chest.");
                return; 
            }

            // If the equipped item is not the required item
            if (equippedItem.id != requiredItem.id)
            {
                Debug.Log("You need a " + requiredItem.itemName + " to open this chest.");
                return;
            }
        }

        // Change sprite to opened state
        SetOpened(true);

        // Open the chest and spawn the item
        OpenChest();
    }

    private void OpenChest()
    {
        IsOpened = true;
        if (ContainedItemPrefab != null)
        {
            Instantiate(ContainedItemPrefab, transform.position + Vector3.up, Quaternion.identity);
        }
    }

    private void SetOpened(bool opened)
    {
        IsOpened = opened;
        if (IsOpened && OpenedSprite != null)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = OpenedSprite;
            }
        }
    }

    public void LoadData(GameData data)
    {
        data.interactedItem.TryGetValue(ChestID, out bool wasOpened);
        if (wasOpened)
        {
            SetOpened(true);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.interactedItem.ContainsKey(ChestID))
        {
            data.interactedItem.Remove(ChestID);
        }
        data.interactedItem.Add(ChestID, IsOpened);
    }
}
