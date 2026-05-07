using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Configuración de Mezclador")]
    public AudioMixer audioMixer;

    [Header("Fuentes de Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        float dbVolume = volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20;
        
        if (audioMixer != null) 
            audioMixer.SetFloat("Music", dbVolume);
        
        PlayerPrefs.SetFloat("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        float dbVolume = volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20;
        
        if (audioMixer != null) 
            audioMixer.SetFloat("SFX", dbVolume);
        
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    public void SetMute(bool isMuted)
    {
        if (isMuted)
        {
            if (audioMixer != null)
            {
                audioMixer.SetFloat("Music", -80f);
                audioMixer.SetFloat("SFX", -80f);
            }
        }
        else
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVol", 0.75f));
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVol", 0.75f));
        }
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
    }

    private void LoadVolumeSettings()
    {
        bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        if (isMuted)
        {
            SetMute(true);
        }
        else
        {
            float musicVol = PlayerPrefs.GetFloat("MusicVol", 0.75f);
            float sfxVol = PlayerPrefs.GetFloat("SFXVol", 0.75f);
            
            SetMusicVolume(musicVol);
            SetSFXVolume(sfxVol);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private AudioClip[] playlistActual;
    private int indiceCancionActual = 0;

    public void PlayMusic(AudioClip[] clips)
    {
        if (musicSource == null || clips == null || clips.Length == 0) return;

        if (playlistActual != null && playlistActual.Length > 0 && clips.Length > 0)
        {
            if (playlistActual[0] == clips[0] && musicSource.isPlaying)
                return; 
        }

        playlistActual = clips;
        indiceCancionActual = 0;
        
        musicSource.clip = playlistActual[indiceCancionActual];
        musicSource.loop = (playlistActual.Length == 1);
        musicSource.Play();
    }

    private void Update()
    {
        if (playlistActual != null && playlistActual.Length > 1 && musicSource != null)
        {
            if (!musicSource.isPlaying)
            {
                indiceCancionActual++;
                if (indiceCancionActual >= playlistActual.Length)
                {
                    indiceCancionActual = 0;
                }

                musicSource.clip = playlistActual[indiceCancionActual];
                musicSource.Play();
            }
        }
    }
}
