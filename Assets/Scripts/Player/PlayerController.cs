using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("configuraciones")]
    public float velocidadMovimiento = 10;
    public float fuerzaDeSalto = 5;
    [Header("Colisiones")]
    public float radioDeColision;
    public Vector2 abajo;
    public LayerMask layerPiso;
    [Header("booleanos")]
    public bool enSuelo = true;
    public bool saltando = false;
    Vector2 direccion;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

    }


    private void FixedUpdate()
    {
        // Verifica si el personaje está tocando el suelo
        Agarres();
        // Maneja el movimiento del personaje
        Caminar();
        // Ejecuta el salto si la bandera de salto está activada
        if (saltando)
        {
            saltar();
        }
    }

    void Update()
    {
        // Detecta la entrada del jugador para el salto
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            // Si está en el suelo, permite el salto
            saltando = true;
        }
        else if (Input.GetButtonDown("Jump") && !enSuelo)
        {
            // Si no está en el suelo, el salto no se ejecuta
            saltando = false;
            Debug.Log("No está en el suelo, NO SALTA");
        }
        else if (Input.GetButtonDown("Jump"))
        {
            // Mensaje de depuración para verificar la condición
            saltando = false;
            Debug.Log("Tecla presionada, NO SALTA " + enSuelo);
        }
    }

    private void Caminar()
    {
        if (enSuelo)
        {
            // Captura la entrada del jugador en los ejes horizontal y vertical
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            // Asigna la dirección del movimiento
            direccion = new Vector2(x, y);

            // Aplica la velocidad de movimiento al Rigidbody2D
            rb.linearVelocity = new Vector2(direccion.x * velocidadMovimiento, rb.linearVelocityY);

            // Cambia la orientación del personaje según la dirección del movimiento
            if (direccion != Vector2.zero)
            {
                if (direccion.x < 0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if (direccion.x > 0 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }

    private void Agarres()
    {
        // Comprueba si el personaje está tocando el suelo usando una superposición circular
        enSuelo = Physics2D.OverlapCircle((Vector2)transform.position + abajo, radioDeColision, layerPiso);
    }

    private void saltar()
    {
        // Aplica una fuerza de impulso hacia arriba para saltar
        rb.AddForce(new Vector2(0, fuerzaDeSalto), ForceMode2D.Impulse);
        // Desactiva la bandera de salto para evitar múltiples saltos
        saltando = false;
        Debug.Log("Saltando");
    }
}
