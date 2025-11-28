using System.Collections;
using UnityEngine;

namespace Codigo.Scripts
{
    public class CamaraSeguimiento : MonoBehaviour
    {
        // variables privadas para guardar a quien seguimos
        private Transform objetivoActual; 
        private Transform objetivoJugador; 

        public float suavizado = 0.125f;
        public Vector3 offset = new Vector3(0f, 10f, -8f);

        public bool activarLimites = true;
        public Vector2 limitesX;

        // interruptor para saber si acaba de cargar la escena
        private bool inicioEscena = true; 

        void Start()
        {
            // busca al jugador
            GameObject jugador = GameObject.FindGameObjectWithTag("Jugador");

            if (jugador != null)
            {
                objetivoJugador = jugador.transform;
                objetivoActual = objetivoJugador; 
                
                // pone la camara en su sitio ya
                transform.position = objetivoActual.position + offset;
                
                // inicia la cuenta atras (para que no se vea un movimiento de la cámara en calcular la distancia
                StartCoroutine(EsperarUnPoco());
            }
        }

        // esto espera medio segundo al iniciar
        IEnumerator EsperarUnPoco()
        {
            inicioEscena = true;
            yield return new WaitForSeconds(0.5f); // pausa de medio segundo
            inicioEscena = false; // ya puede moverse suave
        }

        // funcion para mirar al combate
        public void EnfocarPuntoCombate(Transform nuevoObjetivo)
        {
            objetivoActual = nuevoObjetivo;
            activarLimites = false; // quitar limites para ver completo
        }

        // funcion para volver a mirar al jugador
        public void EnfocarJugador()
        {
            objetivoActual = objetivoJugador;
            activarLimites = true; // poner limites para no ver el vacio
        }

        void LateUpdate()
        {
            if (objetivoActual == null) return; // si no hay nadie, no hace nada, por si acaso

            // calcula donde quiere ir la camara
            Vector3 sitioDeseado = objetivoActual.position + offset;

            // si los limites estan activos, no deja que se salga
            if (activarLimites)
            {
                float xLimitado = Mathf.Clamp(sitioDeseado.x, limitesX.x, limitesX.y);
                sitioDeseado = new Vector3(xLimitado, sitioDeseado.y, sitioDeseado.z);
            }

            // mide cuan lejos esta la camara del destino
            float distancia = Vector3.Distance(transform.position, sitioDeseado);

            // si acaba de empezar o esta muy lejos, salta de golpe
            if (inicioEscena || distancia > 15f) 
            {
                transform.position = sitioDeseado; 
            }
            else
            {
                // si esta cerca, se mueve suave
                transform.position = Vector3.Lerp(transform.position, sitioDeseado, suavizado);
            }
        }
    }
}