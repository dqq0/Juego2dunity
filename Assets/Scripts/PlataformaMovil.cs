using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [Header("Configuración de Coordenadas")]
    [Tooltip("Coordenada X e Y donde inicia la trampa (Punto A)")]
    public Vector2 puntoA;
    [Tooltip("Coordenada X e Y hasta donde llegará la trampa (Punto B)")]
    public Vector2 puntoB;
    
    [Header("Velocidad")]
    public float velocidad = 3f;

    [Header("Tipo de Movimiento")]
    [Tooltip("Marca esta casilla si la trampa se mueve de Arriba a Abajo. Déjala desmarcada si se mueve de Lado a Lado.")]
    public bool esVertical = false;

    [Header("Animaciones de Choque")]
    public string animGolpeIzquierda = "Left Hit (42x42)";
    public string animGolpeDerecha = "Right Hit (42x42)";
    public string animGolpeArriba = "Top Hit (42x42)";
    public string animGolpeAbajo = "Bottom Hit (42x42)";
    public string animacionNormal = "Blink (42x42)";
    [Tooltip("Cuánto tiempo se queda mostrando la cara de choque antes de volver a la normal")]
    public float tiempoMostrandoChoque = 0.3f;

    private Vector2 destinoActual;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        // Al darle Play, la trampa empieza en tu Punto A
        transform.position = puntoA;
        destinoActual = puntoB;
    }

    void FixedUpdate()
    {
        // Mover la plataforma hacia el destino exacto
        transform.position = Vector2.MoveTowards(transform.position, destinoActual, velocidad * Time.fixedDeltaTime);

        // Si llegamos al destino, chocamos
        if (Vector2.Distance(transform.position, destinoActual) < 0.05f)
        {
            Vector2 puntoAnterior = (destinoActual == puntoA) ? puntoB : puntoA;
            
            if (!esVertical)
            {
                if (destinoActual.x < puntoAnterior.x)
                {
                    Debug.Log("Chocó a la Izquierda. Intentando reproducir: " + animGolpeIzquierda);
                    if (anim != null) anim.Play(animGolpeIzquierda);
                }
                else
                {
                    Debug.Log("Chocó a la Derecha. Intentando reproducir: " + animGolpeDerecha);
                    if (anim != null) anim.Play(animGolpeDerecha);
                }
            }
            else
            {
                if (destinoActual.y > puntoAnterior.y)
                {
                    Debug.Log("Chocó Arriba. Intentando reproducir: " + animGolpeArriba);
                    if (anim != null) anim.Play(animGolpeArriba);
                }
                else
                {
                    Debug.Log("Chocó Abajo. Intentando reproducir: " + animGolpeAbajo);
                    if (anim != null) anim.Play(animGolpeAbajo);
                }
            }

            // Cambiamos de dirección
            destinoActual = puntoAnterior;

            // Volvemos a la animación normal después del tiempo configurado
            Invoke("VolverANormal", tiempoMostrandoChoque);
        }
    }

    void VolverANormal()
    {
        if (anim != null) anim.Play(animacionNormal);
    }
}
