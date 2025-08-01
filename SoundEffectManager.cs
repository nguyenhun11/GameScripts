using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider soundEffectSlider;
   
    #region singleton
    public static SoundEffectManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>(); 
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        soundEffectSlider.onValueChanged.AddListener(delegate
        {
            OnValueChange();
        });
    }

    public static void Play(string soundName)
    {
        AudioClip clip = soundEffectLibrary.GetRandomClip(soundName);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void OnValueChange()
    {
        SetVolume(soundEffectSlider.value);
    }
}
