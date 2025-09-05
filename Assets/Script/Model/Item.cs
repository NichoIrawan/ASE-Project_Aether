using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, IDataPersistence
{
    public int id;
    public string itemName;

    private bool isCollected = false;

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

    private void SetItemVisual(bool isVisualActive)
    {
        gameObject.SetActive(!isVisualActive);
    }

    public void LoadData(GameData data)
    {
        if (data.collectedItem.TryGetValue(id, out bool isCollected) && isCollected)
        {
            SetItemVisual(isCollected);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.collectedItem.ContainsKey(id))
        {
            data.collectedItem.Remove(id);
        }
        data.collectedItem.Add(id, isCollected);
    }
}
