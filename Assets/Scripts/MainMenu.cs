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
        LoadVolumeUI();
    }

    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
        Debug.Log("¡El jugador ha salido del juego!");
        Application.Quit();
    }

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
