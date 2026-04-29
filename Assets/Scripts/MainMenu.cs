using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Esta función se conectará al botón de "Jugar" o "Play"
    public void Jugar()
    {
        // Esto carga la siguiente escena en la lista de Build Settings (que debería ser tu Nivel 1)
        // Puedes cambiarlo por SceneManager.LoadScene("NombreDeTuEscena") si lo prefieres
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Esta función se conectará al botón de "Salir" o "Quit"
    public void Salir()
    {
        Debug.Log("¡El jugador ha salido del juego!"); // Esto es para que lo veas en el editor
        Application.Quit(); // Esto cerrará el juego cuando esté compilado
    }
}
