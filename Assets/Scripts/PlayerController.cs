using UnityEngine;

public class Movimiento2D : MonoBehaviour
{
    // Variables ajustables desde el Inspector
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;

    private Rigidbody2D rb;
    private float movimientoHorizontal;

    void Start()
    {
        // Obtenemos el Rigidbody2D al iniciar
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Capturamos el movimiento en el eje X (Teclas A/D o Flechas Izquierda/Derecha)
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // Detectamos si se presiona la tecla de salto (Barra Espaciadora por defecto)
        if (Input.GetButtonDown("Jump"))
        {
            // Le damos un impulso hacia arriba, manteniendo su velocidad horizontal
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }
    }

    void FixedUpdate()
    {
        // Aplicamos el movimiento horizontal
        // Mantenemos rb.velocity.y para no interferir con la gravedad o el salto
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }
}