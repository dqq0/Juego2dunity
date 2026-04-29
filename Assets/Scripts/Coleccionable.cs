using UnityEngine;

public class Coleccionable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador toca este objeto
        if (collision.CompareTag("Player"))
        {
            // Le avisamos al GameManager que recogimos una manzana
            GameManager.Instance.RecogerManzana();

            // Y destruimos la manzana para que desaparezca
            Destroy(gameObject);
        }
    }
}
