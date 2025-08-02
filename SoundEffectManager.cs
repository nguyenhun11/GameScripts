using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static AudioSource audioSource;
    private static AudioSource randomPitchAudioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider soundEffectSlider;
   
    #region singleton
    public static SoundEffectManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>(); 
            audioSource = audioSources[0];
            randomPitchAudioSource = audioSources[1];

            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            //DontDestroyOnLoad(gameObject);
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

    public static void Play(string soundName, bool randomPitch = false)
    {
        AudioClip clip = soundEffectLibrary.GetRandomClip(soundName);
        if (clip != null)
        {
            if (randomPitch)
            {
                randomPitchAudioSource.pitch = Random.Range(0.5f, 1.5f);
                
                audioSource.PlayOneShot(clip);
            }
            else
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        randomPitchAudioSource.volume = volume;
    }

    public void OnValueChange()
    {
        SetVolume(soundEffectSlider.value);
    }
}
