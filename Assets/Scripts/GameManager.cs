using UnityEngine;
using TMPro; // Para el texto normal
using UnityEngine.UI; // Necesario para usar Imágenes en la UI

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Coleccionables")]
    public int manzanasRecogidas = 0;
    public int manzanasNecesarias = 3;

    [Header("UI de Texto (Opcional)")]
    public TextMeshProUGUI textoManzanas;

    [Header("UI con Imágenes (Opcional)")]
    [Tooltip("El objeto de la UI que mostrará el número de manzanas que tienes")]
    public Image imagenNumero;
    [Tooltip("Arrastra aquí los sprites de los números en orden: 0, 1, 2, 3, etc.")]
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

    public void RecogerManzana()
    {
        manzanasRecogidas++;
        Debug.Log("¡Manzana recogida! Llevas: " + manzanasRecogidas + "/" + manzanasNecesarias);
        ActualizarTexto();
    }

    public bool PuedePasarDeNivel()
    {
        return manzanasRecogidas >= manzanasNecesarias;
    }

    private void ActualizarTexto()
    {
        // 1. Actualiza el texto normal si lo tienes
        if (textoManzanas != null)
        {
            textoManzanas.text = "Manzanas: " + manzanasRecogidas + " / " + manzanasNecesarias;
        }

        // 2. Actualiza la imagen del número si la configuraste
        // Verificamos que tengamos la imagen, que el arreglo de sprites no esté vacío
        // y que no nos pasemos de la cantidad de sprites que pusiste
        if (imagenNumero != null && spritesNumeros != null && spritesNumeros.Length > 0)
        {
            if (manzanasRecogidas < spritesNumeros.Length)
            {
                // Cambia la imagen por el sprite correspondiente al número
                imagenNumero.sprite = spritesNumeros[manzanasRecogidas];
            }
        }
    }
}
