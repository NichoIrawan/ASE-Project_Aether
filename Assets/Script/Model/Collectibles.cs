using UnityEngine;

public class Collectibles : Item, IDataPersistence
{
    [ContextMenu("Generate guid")]
    private void GenerateGuid()
    {
        base.id = System.Guid.NewGuid().ToString();
    }

    public override void PickUp()
    {
        GameEventManager.Instance.CandyCollected();
        base.PickUp();
    }

    public override void LoadData(GameData data)
    {
        if (data.collectedCollectibles.TryGetValue(id, out bool isCollected) && isCollected)
        {
            SetItemVisual(isCollected);
        }
    }

    public override void SaveData(ref GameData data)
    {
        if (data.collectedCollectibles.ContainsKey(id))
        {
            data.collectedCollectibles.Remove(id);
        }
        data.collectedCollectibles.Add(id, isCollected);
    }
}
