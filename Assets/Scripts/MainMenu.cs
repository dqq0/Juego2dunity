using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Configuraciones de Audio")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;

    private void Start()
    {
        // Cargar las preferencias de volumen si existen para actualizar la UI
        LoadVolumeUI();
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

    // Estos métodos son llamados por los eventos OnValueChanged de la UI
    public void SetMusicVolume(float volume)
    {
        if (muteToggle != null && muteToggle.isOn) return;
        
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (muteToggle != null && muteToggle.isOn) return;

        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(volume);
    }

    public void SetMute(bool isMuted)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMute(isMuted);
    }

    private void LoadVolumeUI()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVol", 0.75f);
        bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        if (musicSlider != null) musicSlider.value = musicVol;
        if (sfxSlider != null) sfxSlider.value = sfxVol;
        if (muteToggle != null) muteToggle.isOn = isMuted;
    }
}
