using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class LogicaPlayer : MonoBehaviour
{
    [SerializeField] private float velocidadAvance = 8f;
    [SerializeField] private float velocidadLateral = 5f;
    [SerializeField] private GameObject mensajeDerrota;

    [SerializeField] private float velocidadSalto = 5f;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private TMP_Text textoDistancia;

    private Rigidbody cuerpo;
    private BoxCollider colisionador;
    private bool partidaTerminada;
    private bool saltoSolicitado;
    private float zInicial;

    private void Awake()
    {
        cuerpo = GetComponent<Rigidbody>();
        colisionador = GetComponent<BoxCollider>();
        zInicial = transform.position.z;

        if (mensajeDerrota != null)
            mensajeDerrota.SetActive(false);

        ActualizarDistancia();
    }

    private void Update()
    {
        var teclado = Keyboard.current;

        if (partidaTerminada)
        {
            if (teclado != null && teclado.rKey.wasPressedThisFrame)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            return;
        }

        if (teclado != null && teclado.spaceKey.wasPressedThisFrame)
            saltoSolicitado = true;

        ActualizarDistancia();
    }

    private void FixedUpdate()
    {
        if (partidaTerminada)
            return;

        float direccion = 0f;
        var teclado = Keyboard.current;

        if (teclado != null)
        {
            if (teclado.leftArrowKey.isPressed)
                direccion -= 1f;

            if (teclado.rightArrowKey.isPressed)
                direccion += 1f;
        }

        float velocidadVertical = cuerpo.linearVelocity.y;

        if (saltoSolicitado && velocidadVertical <= 0f && EstaEnSuelo())
            velocidadVertical = velocidadSalto;

        saltoSolicitado = false;

        cuerpo.linearVelocity = new Vector3(
            direccion * velocidadLateral,
            velocidadVertical,
            velocidadAvance
        );
    }

    private bool EstaEnSuelo()
    {
        Bounds limites = colisionador.bounds;

        return Physics.Raycast(
            limites.center,
            Vector3.down,
            limites.extents.y + 0.08f,
            capaSuelo,
            QueryTriggerInteraction.Ignore
        );
    }

    private void ActualizarDistancia()
    {
        if (textoDistancia == null)
            return;

        float avance = Mathf.Max(0f, transform.position.z - zInicial);
        textoDistancia.text = $"Distancia: {Mathf.FloorToInt(avance)} m";
    }

    private void OnCollisionEnter(Collision choque)
    {
        if (partidaTerminada || !choque.gameObject.CompareTag("Obstaculo"))
            return;

        partidaTerminada = true;
        cuerpo.linearVelocity = Vector3.zero;
        ActualizarDistancia();

        if (mensajeDerrota != null)
            mensajeDerrota.SetActive(true);
    }
}