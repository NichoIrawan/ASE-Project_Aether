using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }
    public event Action OnCandyCollected;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        Instance = this;
    }

    public void CandyCollected()
    {
        OnCandyCollected?.Invoke();
    }
}