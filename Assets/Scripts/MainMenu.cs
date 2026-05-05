using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Configuraciones de Audio")]
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;

    private void Start()
    {
        // Cargar las preferencias de volumen si existen
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            LoadVolume();
        }
        else
        {
            // Inicializar por defecto
            SetMusicVolume(musicSlider != null ? musicSlider.value : 0.75f);
            SetSFXVolume(sfxSlider != null ? sfxSlider.value : 0.75f);
        }
    }

    public void Jugar()
    {
        // Esto carga la siguiente escena en la lista de Build Settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
        Debug.Log("¡El jugador ha salido del juego!"); // Visible en el editor
        Application.Quit(); // Cierra el juego al estar compilado
    }

    public void SetMusicVolume(float volume)
    {
        if (muteToggle != null && muteToggle.isOn) return;
        
        // Convertir volumen lineal a logarítmico (Decibelios)
        float dbVolume = volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20;
        if (audioMixer != null) audioMixer.SetFloat("Music", dbVolume);
        
        PlayerPrefs.SetFloat("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (muteToggle != null && muteToggle.isOn) return;

        // Convertir volumen lineal a logarítmico (Decibelios)
        float dbVolume = volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20;
        if (audioMixer != null) audioMixer.SetFloat("SFX", dbVolume);
        
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
            // Restaurar volumen guardado
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVol", 0.75f));
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVol", 0.75f));
        }
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
    }

    private void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVol", 0.75f);
        bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        if (musicSlider != null) musicSlider.value = musicVol;
        if (sfxSlider != null) sfxSlider.value = sfxVol;
        if (muteToggle != null) muteToggle.isOn = isMuted;

        // Aplicar el estado de mute
        SetMute(isMuted);
    }
}
