using UnityEngine;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour, IDataPersistence
{
    public string id;
    public string itemName;

    public bool isCollected { get; private set; }

    private void Awake()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.Register(this);
        }
    }

    private void OnDestroy()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.Unregister(this);
        }
    }

    [ContextMenu("Generate guid")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void PickUp()
    {
        GameEventManager.Instance.CandyCollected();
        SoundEffectManager.PlaySound("Collect");
        PlayerScript.CollectedCandy++;

        Sprite itemIcon = GetComponent<Image>().sprite;

        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(itemName, itemIcon);
        }

        // Disable the item in the scene
        isCollected = true;
        SetItemVisual(!isCollected);
    }

    protected void SetItemVisual(bool isVisualActive)
    {
        gameObject.SetActive(isVisualActive);
    }

    public void LoadData(GameData data)
    {
        if (data.collectedCollectibles.TryGetValue(id, out bool isCollect) && isCollect)
        {
            isCollected = isCollect;
            SetItemVisual(!isCollected);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.collectedCollectibles.ContainsKey(id))
        {
            data.collectedCollectibles.Remove(id);
        }
        data.collectedCollectibles.Add(id, isCollected);
    }
}
