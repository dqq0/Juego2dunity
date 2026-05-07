using UnityEngine;
using UnityEngine.SceneManagement; // Importante: Esto nos permite reiniciar la escena

public class MovimientoPatrulla : MonoBehaviour
{
    [Header("Configuración de Coordenadas")]
    [Tooltip("Coordenada X e Y donde inicia la sierra")]
    public Vector2 puntoA;
    [Tooltip("Coordenada X e Y hasta donde llegará la sierra")]
    public Vector2 puntoB;
    public float velocidad = 3f;

    private Vector2 destinoActual;

    void Start()
    {
        // Al darle Play, la sierra se teletransporta a tu Punto A y empieza a ir al Punto B
        transform.position = puntoA;
        destinoActual = puntoB;
    }

    void Update()
    {
        // Movimiento basado en coordenadas exactas
        transform.position = Vector2.MoveTowards(transform.position, destinoActual, velocidad * Time.deltaTime);

        // Cambio de dirección
        if (Vector2.Distance(transform.position, destinoActual) < 0.1f)
        {
            if (destinoActual == puntoB)
            {
                destinoActual = puntoA;
            }
            else
            {
                destinoActual = puntoB;
            }
        }
    }

    // Aquí detectamos cuando el jugador entra en el área del Is Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprobamos si el objeto que entró tiene la etiqueta (Tag) "Player"
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡La sierra cortó al jugador!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PerderJuego();
            }
            else
            {
                // Respaldo por si no hay GameManager
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}