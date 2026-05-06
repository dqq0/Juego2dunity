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

    [Header("Estados del Juego (Ganar/Perder)")]
    public GameObject panelGanar;
    public GameObject panelPerder;
    private bool juegoTerminado = false;

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
        
        // Asegurarnos de que al iniciar el juego, los paneles estén desactivados
        if (menuPausaPanel != null) menuPausaPanel.SetActive(false);
        if (panelGanar != null) panelGanar.SetActive(false);
        if (panelPerder != null) panelPerder.SetActive(false);
        
        Time.timeScale = 1f; 
        juegoTerminado = false;
    }

    private void Update()
    {
        // Detectar cuando se presiona la tecla Escape, solo si el juego no ha terminado
        if (Input.GetKeyDown(KeyCode.Escape) && !juegoTerminado)
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
        if (juegoTerminado) return; // No pausar si ya ganamos o perdimos

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

    public void GanarJuego()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;
        
        if (panelGanar != null) panelGanar.SetActive(true);
        Time.timeScale = 0f; // Congelamos el juego al ganar
        
        // Opcional: Reproducir sonido de victoria aquí
        // if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX(sonidoVictoria);
    }

    public void PerderJuego()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;
        
        if (panelPerder != null) panelPerder.SetActive(true);
        Time.timeScale = 0f; // Congelamos el juego al perder
        
        // Opcional: Reproducir sonido de derrota aquí
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f; // IMPORTANTE: Descongelar el tiempo antes de recargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SiguienteNivel()
    {
        Time.timeScale = 1f; // Descongelar el tiempo
        int siguienteNivel = SceneManager.GetActiveScene().buildIndex + 1;
        int totalEscenas = SceneManager.sceneCountInBuildSettings;
        
        Debug.Log($"Intentando pasar al nivel índice: {siguienteNivel}. Total de escenas en Build Settings: {totalEscenas}");
        
        if (siguienteNivel < totalEscenas)
        {
            Debug.Log("Cargando la escena con índice: " + siguienteNivel);
            SceneManager.LoadScene(siguienteNivel);
        }
        else
        {
            Debug.Log("No hay más niveles registrados en Build Settings. Volviendo al menú (0).");
            // Si ya no hay más niveles, volver al menú
            SceneManager.LoadScene(0); 
        }
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
