using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CandyCollectedText : MonoBehaviour, IDataPersistence
{
    [SerializeField]private int TotalCandy = 0;
    
    private int candyCollected = 0;
    private TextMeshProUGUI text;

    private void Awake()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.Register(this);
        }
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.Unregister(this);
        }
        GameEventManager.Instance.OnCandyCollected -= OnCandyCollected;
    }


    void Start()
    {
        GameEventManager.Instance.OnCandyCollected += OnCandyCollected;
    }


    void Update()
    {
        text.text = candyCollected + " / " + TotalCandy;
    }

    private void OnCandyCollected()
    {
        candyCollected++;
    }

    public void LoadData(GameData data)
    {
        candyCollected = 0;

        foreach (KeyValuePair<string, bool> pair in data.collectedCollectibles)
        {
            if (pair.Value)
            {
                candyCollected++;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        // Do Nothing
    }
}
