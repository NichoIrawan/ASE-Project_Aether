using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, IDataPersistence
{
    public string id;
    public string itemName;

    protected bool isCollected = false;

    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;

        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(itemName, itemIcon);
        }

        // Disable the item in the scene
        isCollected = true;
        SetItemVisual(isCollected);
    }

    public virtual void Use()
    {
        Debug.Log("Using item: " + itemName);
    }

    protected void SetItemVisual(bool isVisualActive)
    {
        gameObject.SetActive(!isVisualActive);
    }

    public virtual void LoadData(GameData data)
    {
        if (data.collectedItem.TryGetValue(id, out bool isCollected) && isCollected)
        {
            SetItemVisual(isCollected);
        }
    }

    public virtual void SaveData(ref GameData data)
    {
        if (data.collectedItem.ContainsKey(id))
        {
            data.collectedItem.Remove(id);
        }
        data.collectedItem.Add(id, isCollected);
    }
}
