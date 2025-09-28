using UnityEngine;

public class CollectibleItem : Item, IDataPersistence
{
    public string itemUID;

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
        itemUID = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {
        if (data.collectedItem.TryGetValue(itemUID, out bool isCollect) && isCollect)
        {
            isCollected = isCollect;
            SetItemVisual(!isCollected);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.collectedItem.ContainsKey(itemUID))
        {
            data.collectedItem.Remove(itemUID);
        }
        data.collectedItem.Add(itemUID, isCollected);
    }
}
