using UnityEngine;

public class CamaraSeguidora : MonoBehaviour
{
    [SerializeField] private Transform jugador;

    private Vector3 posicionInicialCamara;
    private float zInicialJugador;

    private void Start()
    {
        if (jugador == null)
        {
            Debug.LogWarning("Asigna el Player a la cámara en el Inspector.", this);
            enabled = false;
            return;
        }

        posicionInicialCamara = transform.position;
        zInicialJugador = jugador.position.z;
    }

    private void LateUpdate()
    {
        float avance = jugador.position.z - zInicialJugador;
        transform.position = posicionInicialCamara + Vector3.forward * avance;
    }
}
