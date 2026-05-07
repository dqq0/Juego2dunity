using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Coleccionables")]
    public bool requiereFrutasParaAvanzar = false;
    public int frutasRecogidas = 0;
    public int frutasNecesarias = 3;

    [Header("UI de Texto")]
    public TextMeshProUGUI textoFrutas;

    [Header("UI con Imágenes")]
    public Image imagenNumero;
    public Sprite[] spritesNumeros;

    [Header("Menú de Pausa")]
    public GameObject menuPausaPanel;
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
        
        if (menuPausaPanel != null) menuPausaPanel.SetActive(false);
        if (panelGanar != null) panelGanar.SetActive(false);
        if (panelPerder != null) panelPerder.SetActive(false);
        
        Time.timeScale = 1f; 
        juegoTerminado = false;
    }

    private void Update()
    {
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
        if (juegoTerminado) return;

        juegoPausado = true;
        Time.timeScale = 0f; 
        
        if (menuPausaPanel != null)
        {
            menuPausaPanel.SetActive(true);
        }
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        
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
        Time.timeScale = 0f; 
    }

    public void PerderJuego()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;
        
        if (panelPerder != null) panelPerder.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SiguienteNivel()
    {
        Time.timeScale = 1f;
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
            SceneManager.LoadScene(0); 
        }
    }

    public void SalirAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
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
