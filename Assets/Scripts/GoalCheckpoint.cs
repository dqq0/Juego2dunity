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
            // Verificamos si recogió todas las frutas (si es obligatorio)
            if (GameManager.Instance != null && !GameManager.Instance.PuedePasarDeNivel())
            {
                Debug.Log("¡Aún te faltan frutas para poder terminar el nivel!");
                return; // No pasa de nivel aún
            }

            nivelCompletado = true;
            
            // 1. Activamos la animación que configuramos
            anim.SetTrigger("Activate");
            
            Debug.Log("¡Nivel superado! Mostrando panel de victoria.");

            // 2. Ejecutar la función para mostrar panel de ganar
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GanarJuego();
            }
        }
    }
}