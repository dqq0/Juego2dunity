using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de nivel

public class GoalCheckpoint : MonoBehaviour
{
    private Animator anim;
    private bool nivelCompletado = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Solo se activa si es el Jugador y si no lo hemos activado ya
        if (collision.CompareTag("Player") && !nivelCompletado)
        {
            // Verificamos si recogió todas las manzanas usando el GameManager
            if (GameManager.Instance != null && GameManager.Instance.PuedePasarDeNivel())
            {
                nivelCompletado = true;
                
                // 1. Activamos la animación que configuramos
                anim.SetTrigger("Activate");
                
                Debug.Log("¡Nivel superado! Aquí cargaríamos el siguiente nivel.");

                // 2. Ejecutar la función para pasar de nivel tras un pequeño retraso
                Invoke("PasarDeNivel", 2f); 
            }
            else
            {
                // Mensaje en caso de que falten manzanas
                Debug.Log("¡Aún te faltan manzanas para poder terminar el nivel!");
            }
        }
    }

    void PasarDeNivel()
    {
        // Por ahora, como no tienes más niveles, reiniciará el mismo nivel
        // Cuando tengas el Nivel 2, cambiarás "SampleScene" por el nombre de tu siguiente nivel
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        // El comando real para el futuro sería:
        // SceneManager.LoadScene("NombreDeTuNivel2");
    }
}