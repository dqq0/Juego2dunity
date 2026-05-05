using UnityEngine;
using TMPro; // Para el texto normal
using UnityEngine.UI; // Necesario para usar Imágenes en la UI

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
