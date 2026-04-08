using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Movimiento2D : MonoBehaviour
{
    [Header("Movimiento (Horizontal)")]
    public float velocidadMaxima = 10f;
    public float aceleracion = 50f;
    public float desaceleracion = 50f;
    [Range(0.01f, 1f)] public float agilidadEnElAire = 0.65f;

    [Header("Salto Principal")]
    public float fuerzaSalto = 20f;
    [Tooltip("Controla qué tan rápido cae (sensación Meat Boy)")]
    public float multiplicadorGravedadCaida = 2.5f;
    [Tooltip("Controla el mini-salto si sueltas el botón rápido")]
    public float multiplicadorGravedadCorto = 2f;
    
    [Header("Ventanas de Gracia (Game Feel)")]
    [Tooltip("Tiempo para saltar justo después de caerse de la cornisa")]
    public float tiempoCoyote = 0.1f;
    [Tooltip("El juego 'recuerda' si apretaste salto un milisegundo antes de aterrizar")]
    public float tiempoBufferSalto = 0.1f;

    [Header("Salto de Muro (Wall Jump)")]
    public float velocidadDeslizamientoMuro = 2f;
    public Vector2 poderSaltoMuro = new Vector2(10f, 15f);

    [Header("OBLIGATORIO: Detección")]
    [Tooltip("Debes seleccionar aquí la capa (Layer) en la que está el Piso")]
    public LayerMask capaPiso;

    // Privadas
    private Rigidbody2D rb;
    private Collider2D miCollider;

    private float _ejeX;
    private bool _enSuelo;
    private bool _tocandoMuroDer;
    private bool _tocandoMuroIzq;
    private bool _deslizandoEnMuro;

    private float _coyoteCounter;
    private float _bufferSaltoCounter;
    private bool _mirandoDerecha = true;
    
    // Timer para que el salto de pared sea balístico y parabólico
    private float _bloqueoControlCounter;

    // Animaciones
    private Animator _animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        miCollider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        
        rb.freezeRotation = true; 
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        _ejeX = Input.GetAxisRaw("Horizontal");

        VerificarColisiones();
        GestionarTimers();
        GestionarSalto();
        GestionarDeslizamientoMuro();
        Voltear();
        
        GestionarAnimaciones();
    }

    void FixedUpdate()
    {
        AplicarMovimientoHorizontal();
        ModificarGravedad();
    }

    private void GestionarTimers()
    {
        if (_enSuelo)
            _coyoteCounter = tiempoCoyote;
        else
            _coyoteCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            _bufferSaltoCounter = tiempoBufferSalto;
        else
            _bufferSaltoCounter -= Time.deltaTime;
            
        if (_bloqueoControlCounter > 0)
            _bloqueoControlCounter -= Time.deltaTime;
    }

    private void GestionarSalto()
    {
        // 1. Salto Normal
        if (_bufferSaltoCounter > 0f && _coyoteCounter > 0f && !_deslizandoEnMuro)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            _bufferSaltoCounter = 0f;
            _coyoteCounter = 0f;
        }
        // 2. Wall Jump
        else if (_bufferSaltoCounter > 0f && _deslizandoEnMuro)
        {
            float direccionRebote = _tocandoMuroDer ? -1f : 1f;
            
            // Usamos estrictamente lo que configure el usuario en el Inspector
            rb.linearVelocity = new Vector2(poderSaltoMuro.x * direccionRebote, poderSaltoMuro.y);
            
            Vector3 esc = transform.localScale;
            esc.x = Mathf.Abs(esc.x) * direccionRebote;
            transform.localScale = esc;
            _mirandoDerecha = (direccionRebote == 1);
            
            _bufferSaltoCounter = 0f;
            // Bloqueo cortísimo, suficiente para hacer el arco sin flotar lejos
            _bloqueoControlCounter = 0.1f; 
        }

        // Variabilidad del salto
        if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            _coyoteCounter = 0f;
        }
    }

    private void AplicarMovimientoHorizontal()
    {
        float velocidadObjetivo = _ejeX * velocidadMaxima;
        float ajusteAcel = (Mathf.Abs(velocidadObjetivo) > 0.01f) ? aceleracion : desaceleracion;
        
        if (!_enSuelo) ajusteAcel *= agilidadEnElAire; 
        
        // Si acabamos de saltar desde una pared, perdemos severamente el control en el aire instántaneo para dar ese "Game Feel" balístico pesado.
        if (_bloqueoControlCounter > 0)
        {
            ajusteAcel *= 0.1f; // Pierdes el 90% de control sobre la flecha, forzándote a mantener la trayectoria física.
        }

        float velocidadX = Mathf.MoveTowards(rb.linearVelocity.x, velocidadObjetivo, ajusteAcel * Time.fixedDeltaTime);
        
        rb.linearVelocity = new Vector2(velocidadX, rb.linearVelocity.y);
    }

    private void ModificarGravedad()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (multiplicadorGravedadCaida - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !(Input.GetButton("Jump") || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (multiplicadorGravedadCorto - 1) * Time.fixedDeltaTime;
        }
    }

    private void GestionarDeslizamientoMuro()
    {
        if ((_tocandoMuroIzq || _tocandoMuroDer) && !_enSuelo && rb.linearVelocity.y < 0 && _ejeX != 0)
        {
            _deslizandoEnMuro = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -velocidadDeslizamientoMuro, float.MaxValue));
        }
        else
        {
            _deslizandoEnMuro = false;
        }
    }

    private void VerificarColisiones()
    {
        Bounds limites = miCollider.bounds;
        
        // Disparamos 3 láseres hacia abajo (Izquierda, Centro, Derecha)
        float distanciaRayo = 0.15f;
        Vector2 origenCentro = new Vector2(limites.center.x, limites.min.y + 0.02f);
        Vector2 origenIzq = new Vector2(limites.min.x + 0.1f, limites.min.y + 0.02f);
        Vector2 origenDer = new Vector2(limites.max.x - 0.1f, limites.min.y + 0.02f);

        _enSuelo = LanzaRayo(origenCentro, Vector2.down, distanciaRayo) ||
                   LanzaRayo(origenIzq, Vector2.down, distanciaRayo) ||
                   LanzaRayo(origenDer, Vector2.down, distanciaRayo);

        // Muros (1 láser a cada lado)
        Vector2 centroMuroIzq = new Vector2(limites.min.x + 0.02f, limites.center.y);
        Vector2 centroMuroDer = new Vector2(limites.max.x - 0.02f, limites.center.y);
        float distanciaMuro = 0.15f;

        _tocandoMuroIzq = LanzaRayo(centroMuroIzq, Vector2.left, distanciaMuro);
        _tocandoMuroDer = LanzaRayo(centroMuroDer, Vector2.right, distanciaMuro);
    }

    private bool LanzaRayo(Vector2 origen, Vector2 direccion, float distancia)
    {
        RaycastHit2D[] choques = Physics2D.RaycastAll(origen, direccion, distancia, capaPiso);
        foreach (RaycastHit2D choque in choques)
        {
            if (choque.collider.gameObject != this.gameObject && !choque.collider.isTrigger) 
                return true;
        }
        return false;
    }

    // ¡ESTO ES NUEVO! Te dibujará los láseres en la pantalla del editor para que veas si tocan el piso.
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (miCollider == null) return;

        Bounds limites = miCollider.bounds;
        Gizmos.color = _enSuelo ? Color.green : Color.red;

        // Centro
        Gizmos.DrawRay(new Vector2(limites.center.x, limites.min.y + 0.02f), Vector2.down * 0.15f);
        // Izq
        Gizmos.DrawRay(new Vector2(limites.min.x + 0.1f, limites.min.y + 0.02f), Vector2.down * 0.15f);
        // Der
        Gizmos.DrawRay(new Vector2(limites.max.x - 0.1f, limites.min.y + 0.02f), Vector2.down * 0.15f);
    }

    private void Voltear()
    {
        Vector3 escala = transform.localScale;

        if (_ejeX > 0 && !_mirandoDerecha)
        {
            escala.x = Mathf.Abs(escala.x); // Conservamos tu escala (2, 3, etc) pero positiva
            transform.localScale = escala;
            _mirandoDerecha = true;
        }
        else if (_ejeX < 0 && _mirandoDerecha)
        {
            escala.x = -Mathf.Abs(escala.x); // Congervamos tu escala, negativa
            transform.localScale = escala;
            _mirandoDerecha = false;
        }
    }

    private void GestionarAnimaciones()
    {
        if (_animator == null) return;

        // 1. Si estamos firmes en el suelo
        if (_enSuelo)
        {
            if (Mathf.Abs(_ejeX) > 0.1f)
            {
                _animator.Play("caminar");
            }
            else
            {
                _animator.Play("New Animation"); // Tu quieto
            }
        }
        // 2. Si estamos en el aire
        else 
        {
            // ¿Estamos agarrados a un muro resbalando?
            if (_deslizandoEnMuro)
            {
                _animator.Play("wall");
            }
            // Si la velocidad vertical nos empuja hacia arriba, estamos saltando
            else if (rb.linearVelocity.y > 0.1f)
            {
                _animator.Play("jump");
            }
            // Si la velocidad es gravedad y vamos para abajo, estamos cayendo
            else 
            {
                _animator.Play("fall");
            }
        }
    }
}