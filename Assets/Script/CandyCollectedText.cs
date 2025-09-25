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
        text = GetComponent<TextMeshProUGUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEventManager.Instance.OnCandyCollected += OnCandyCollected;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = candyCollected + " / " + TotalCandy;
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.OnCandyCollected -= OnCandyCollected;
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
