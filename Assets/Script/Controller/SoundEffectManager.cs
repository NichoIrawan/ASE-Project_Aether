using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource[] audioSources;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There are more than 1 instances. Destroying the new one");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        soundEffectLibrary = GetComponent<SoundEffectLibrary>();

        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public static void PlaySound(string name)
    {
        AudioClip clip = soundEffectLibrary.GetRandomClip(name);
        if (clip != null)
        {
            audioSources[0].PlayOneShot(clip);
        }
    }

    public static void SetVolume(float volume)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = volume * 0.75f;
        }
        audioSources[1].volume = volume * 0.15f;
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
