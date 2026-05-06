using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class LevelMusic : MonoBehaviour
{
    [Header("Canciones del Nivel")]
    [Tooltip("Arrastra aquí las canciones que quieres que suenen en este nivel")]
    public AudioClip[] canciones;

    [Header("Configuración (Opcional)")]
    [Tooltip("Arrastra aquí tu AudioMixerGroup llamado 'Music' para que el control de volumen funcione")]
    public AudioMixerGroup musicMixerGroup;

    private AudioSource audioSource;
    private int cancionActual = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Asignar el grupo del mixer si está disponible
        if (musicMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = musicMixerGroup;
        }

        // Si solo hay 1 canción, usamos el loop nativo de Unity que no tiene pausas
        if (canciones.Length == 1)
        {
            audioSource.loop = true;
        }

        // Si hay canciones, reproducir la primera
        if (canciones.Length > 0)
        {
            ReproducirCancion(0);
        }
    }

    void Update()
    {
        // Solo revisamos si terminó la canción si hay MÁS de 1 canción
        if (canciones.Length > 1 && !audioSource.isPlaying)
        {
            // Pasar a la siguiente canción
            cancionActual++;
            
            // Si llegamos al final de la lista, volver a la primera
            if (cancionActual >= canciones.Length)
            {
                cancionActual = 0;
            }

            ReproducirCancion(cancionActual);
        }
    }

    private void ReproducirCancion(int indice)
    {
        audioSource.clip = canciones[indice];
        audioSource.Play();
    }
}
