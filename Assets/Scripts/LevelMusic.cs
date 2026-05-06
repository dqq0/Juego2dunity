using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(10)] // Asegura que se ejecute después del AudioManager
public class LevelMusic : MonoBehaviour
{
    [Header("Canciones del Nivel")]
    [Tooltip("Arrastra aquí las canciones que quieres que suenen en esta escena")]
    public AudioClip[] canciones;

    void Start()
    {
        // Enviar la lista de canciones de este nivel al AudioManager (Singleton)
        if (AudioManager.Instance != null && canciones.Length > 0)
        {
            AudioManager.Instance.PlayMusic(canciones);
        }
        else if (AudioManager.Instance == null)
        {
            Debug.LogWarning("Falta el AudioManager en la escena inicial (ve al Menú y dale Play desde ahí).");
        }
    }
}
