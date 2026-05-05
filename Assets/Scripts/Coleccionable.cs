using UnityEngine;

public class Coleccionable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador toca este objeto
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RecogerFruta();
            }

            // Y destruimos la fruta para que desaparezca
            Destroy(gameObject);
        }
    }
}
