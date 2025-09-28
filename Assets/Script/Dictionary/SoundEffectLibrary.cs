using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach (SoundEffectGroup group in soundEffectGroups)
        {
            soundDictionary[group.name] = group.clips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (soundDictionary.TryGetValue(name, out List<AudioClip> clips) && clips.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, clips.Count);
            return clips[randomIndex];
        }

        Debug.LogWarning($"Sound group '{name}' not found or has no clips.");
        return null;
    }

    [Serializable]
    public struct SoundEffectGroup
    {
        public string name;
        public List<AudioClip> clips;
    }
}
