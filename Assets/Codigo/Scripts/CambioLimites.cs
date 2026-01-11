using UnityEngine;

public class CambiadorLimites : MonoBehaviour
{
    public float nuevoLimiteIzquierdo;
    public float nuevoLimiteDerecho;

    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto que entra tiene el Tag "Player"
        if (other.CompareTag("Player"))
        {
            Jugador scriptJugador = other.GetComponent<Jugador>();
            if (scriptJugador != null)
            {
                scriptJugador.limiteIzquierdo = nuevoLimiteIzquierdo;
                scriptJugador.limiteDerecho = nuevoLimiteDerecho;
                Debug.Log("Límites de sala actualizados");
            }
        }
    }
}