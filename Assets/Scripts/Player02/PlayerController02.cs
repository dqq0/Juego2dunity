using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController02 : MonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer sprRender;
    public Animator animator;
    [Header("configuraciones")]
    public float velocidadMovimiento = 10;
    public float fuerzaDeSalto = 5;
    public float tiempoMorir = 10f;
    [Header("Colisiones")]
    public float radioDeColision =0.1f;
    public Vector2 abajo = new Vector2(0,-0.65f);
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
        StartCoroutine(MorirEnSegundos());
    }

    private void FixedUpdate()
    {
        Agarres();
        Caminar();
        if (saltando)
        {
            saltar();
        }
    }
    // Update is called once per frame
    void Update()
    {
      

        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            saltando = true;

        }
        else if (Input.GetButtonDown("Jump") && !enSuelo)
        {
            saltando = false;
            Debug.Log("no esta en el suelo NO SALTA");
        }
        else if (Input.GetButtonDown("Jump"))
        {
            saltando = false;
            Debug.Log("tecla presionada NO SALTA " + enSuelo);
        }

        if (!enSuelo)
        {
            animator.SetBool("saltar", true);
        }else
        {
            animator.SetBool("saltar", false);
        }
        if(enSuelo && rb.linearVelocity.x != 0f)
        {
            animator.SetBool("caminar", true);
        }
        else
        {
            animator.SetBool("caminar", false);
        }
    }

    private void Caminar()
    {
        if (enSuelo)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            direccion = new Vector2(x, y);

            rb.linearVelocity = new Vector2(direccion.x * velocidadMovimiento, direccion.y);
            if (direccion != Vector2.zero)
            {
                if (direccion.x < 0 )
                {
                   // transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                   sprRender.flipX = true;
                    Debug.Log("voy a la izq");

                }
                else if (direccion.x > 0 )
                {
                   // transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    sprRender.flipX = false;
                    Debug.Log("voy a la derecha");
                }
            }

        }

    }

    private void Agarres()
    {
        enSuelo = Physics2D.OverlapCircle((Vector2)transform.position + abajo, radioDeColision, layerPiso);

    }
    private void saltar()
    {
        rb.AddForce(new Vector2(0, fuerzaDeSalto), ForceMode2D.Impulse);
        saltando = false;
        Debug.Log("Saltando");
    }

    IEnumerator MorirEnSegundos()
    {
      
        yield return new WaitForSeconds(tiempoMorir);
        animator.SetBool("morir", true);

    }
}
