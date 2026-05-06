using UnityEngine;
using TMPro; // Para el texto normal
using UnityEngine.UI; // Necesario para usar Imágenes en la UI
using UnityEngine.SceneManagement; // Necesario para reiniciar y salir

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Coleccionables")]
    [Tooltip("¿Se deben recoger todas las frutas para poder pasar el nivel?")]
    public bool requiereFrutasParaAvanzar = false;
    public int frutasRecogidas = 0;
    public int frutasNecesarias = 3;

    [Header("UI de Texto (Opcional)")]
    public TextMeshProUGUI textoFrutas;

    [Header("UI con Imágenes (Opcional)")]
    public Image imagenNumero;
    public Sprite[] spritesNumeros;

    [Header("Menú de Pausa")]
    public GameObject menuPausaPanel; // Arrastra aquí tu panel de pausa
    private bool juegoPausado = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ActualizarTexto();
        
        // Asegurarnos de que al iniciar el juego, la pausa esté desactivada
        if (menuPausaPanel != null)
        {
            menuPausaPanel.SetActive(false);
        }
        Time.timeScale = 1f; 
    }

    private void Update()
    {
        // Detectar cuando se presiona la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                ReanudarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    public void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f; // Esto congela todos los movimientos físicos y animaciones
        
        if (menuPausaPanel != null)
        {
            menuPausaPanel.SetActive(true);
        }
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f; // Vuelve el tiempo a la normalidad
        
        if (menuPausaPanel != null)
        {
            menuPausaPanel.SetActive(false);
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f; // IMPORTANTE: Descongelar el tiempo antes de recargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirAlMenuPrincipal()
    {
        Time.timeScale = 1f; // Descongelar el tiempo
        SceneManager.LoadScene(0); // El índice 0 debe ser tu escena MenuPrincipal
    }

    public void RecogerFruta()
    {
        frutasRecogidas++;
        Debug.Log("¡Fruta recogida! Llevas: " + frutasRecogidas + "/" + frutasNecesarias);
        ActualizarTexto();
    }

    public bool PuedePasarDeNivel()
    {
        if (!requiereFrutasParaAvanzar)
            return true;
            
        return frutasRecogidas >= frutasNecesarias;
    }

    private void ActualizarTexto()
    {
        if (textoFrutas != null)
        {
            textoFrutas.text = "Frutas: " + frutasRecogidas + " / " + frutasNecesarias;
        }

        if (imagenNumero != null && spritesNumeros != null && spritesNumeros.Length > 0)
        {
            if (frutasRecogidas < spritesNumeros.Length)
            {
                imagenNumero.sprite = spritesNumeros[frutasRecogidas];
            }
        }
    }
}
