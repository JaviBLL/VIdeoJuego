using System.Collections.Generic;
using UnityEngine;

public class GeneradorPista : MonoBehaviour
{
    [SerializeField] private Transform jugador;
    [SerializeField] private Transform tramoInicial;
    [SerializeField] private GameObject prefabTramo;
    [SerializeField, Min(1f)] private float largoTramo = 50f;

    private readonly Queue<GameObject> tramosGenerados = new();
    private Vector3 posicionSiguiente;

    private void Start()
    {
        if (jugador == null || tramoInicial == null || prefabTramo == null || largoTramo <= 0f)
        {
            Debug.LogWarning("Revisa los campos de GeneradorPista en el Inspector.", this);
            enabled = false;
            return;
        }

        posicionSiguiente = tramoInicial.position + Vector3.forward * largoTramo;
    }

    private void Update()
    {
        // Mantiene pista disponible por delante del jugador.
        while (posicionSiguiente.z - jugador.position.z < largoTramo * 2f)
        {
            GameObject tramo = Instantiate(
                prefabTramo,
                posicionSiguiente,
                tramoInicial.rotation
            );

            tramosGenerados.Enqueue(tramo);
            posicionSiguiente += Vector3.forward * largoTramo;
        }

        // Borra las copias que ya quedaron lejos por detrás.
        while (tramosGenerados.Count > 0 &&
               jugador.position.z - tramosGenerados.Peek().transform.position.z > largoTramo * 2f)
        {
            Destroy(tramosGenerados.Dequeue());
        }
    }
}