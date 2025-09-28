using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string id;
    public string itemName;

    public bool isCollected {  get; protected set; }

    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;

        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(itemName, itemIcon);
        }

        // Disable the item in the scene
        isCollected = true;

        // Play sound effect
        SoundEffectManager.PlaySound("Collect");
        SetItemVisual(!isCollected);
    }

    public virtual void Use()
    {
        Debug.Log("Using item: " + itemName);
    }

    protected void SetItemVisual(bool isVisualActive)
    {
        gameObject.SetActive(isVisualActive);
    }
}
