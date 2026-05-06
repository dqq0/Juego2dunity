using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // === SINGLETON ===
    public static AudioManager Instance { get; private set; }

    [Header("Configuración de Mezclador")]
    public AudioMixer audioMixer;

    [Header("Fuentes de Audio (Opcional)")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    private void Awake()
    {
        // Implementación del patrón Singleton con persistencia entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evita que se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruye duplicados
        }
    }

    private void Start()
    {
        // Cargar los volúmenes guardados cuando el juego inicia
        LoadVolumeSettings();
    }

    // ================= MÉTODOS DE VOLUMEN =================

    public void SetMusicVolume(float volume)
    {
        // Convertir lineal a logarítmico (Decibelios)
        float dbVolume = volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20;
        
        if (audioMixer != null) 
            audioMixer.SetFloat("Music", dbVolume);
        
        PlayerPrefs.SetFloat("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        // Convertir lineal a logarítmico (Decibelios)
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
            // Restaurar volumen guardado si quitamos el mute
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

    // ================= MÉTODOS DE REPRODUCCIÓN (Opcionales para la pauta) =================

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Nuevo sistema para manejar una canción o una playlist por escena
    private AudioClip[] playlistActual;
    private int indiceCancionActual = 0;

    public void PlayMusic(AudioClip[] clips)
    {
        if (musicSource == null || clips == null || clips.Length == 0) return;

        // Si es la misma lista y la misma canción ya está sonando, no hacemos nada (evita reinicios al morir)
        if (playlistActual != null && playlistActual.Length > 0 && clips.Length > 0)
        {
            if (playlistActual[0] == clips[0] && musicSource.isPlaying)
                return; 
        }

        playlistActual = clips;
        indiceCancionActual = 0;
        
        musicSource.clip = playlistActual[indiceCancionActual];
        
        // Si solo hay una canción, usamos el loop nativo
        musicSource.loop = (playlistActual.Length == 1);
        musicSource.Play();
    }

    private void Update()
    {
        // Revisar si la canción terminó para pasar a la siguiente en la playlist
        if (playlistActual != null && playlistActual.Length > 1 && musicSource != null)
        {
            if (!musicSource.isPlaying)
            {
                indiceCancionActual++;
                if (indiceCancionActual >= playlistActual.Length)
                {
                    indiceCancionActual = 0; // Volver a empezar la playlist
                }

                musicSource.clip = playlistActual[indiceCancionActual];
                musicSource.Play();
            }
        }
    }
}
