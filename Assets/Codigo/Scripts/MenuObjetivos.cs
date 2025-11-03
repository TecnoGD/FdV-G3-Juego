using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

namespace Codigo.Scripts
{
    public class MenuObjetivos : MonoBehaviour
    {
        public GameObject prefabBotonObjetivo;  // Prefab del boton del objetivo
        public GameObject prefabBotonAtras;     // Prefab del boton para volver al menu anterior
        public int objetivosSeleccionados = 0;  // Número de objetivos seleccionados por el jugador
        public int objetivosMaximos = 0;        // Número máximo de objetivos que se pueden seleccionar

        /* Metodo que se ejecuta cada vez que se habilita el menu de objetivos, el cual generara los toggle para
           seleccionar ls objetivos*/
        public void OnEnable()
        {
            if (GLOBAL.enCombate)
            {
                for (int i = 1; i < SistemaCombate.luchadores.Count; i++)
                {
                    // Inicializa el toggle del objetivo y obtiene su componente
                    Toggle toggleObjetivo = Instantiate(prefabBotonObjetivo, gameObject.transform).GetComponent<Toggle>();
                    // Vincula el evento de onValueChanged del toggle a un metodo delegado que llama a otro metodo 
                    // que maneja la seleccion de objetivos
                    toggleObjetivo.onValueChanged.AddListener(delegate { SeleccionaObjetivo(toggleObjetivo); });
                    // Cambia el nombre del toggle por el del luchador asignado
                    toggleObjetivo.GetComponentInChildren<Text>().text = SistemaCombate.luchadores[i].nombre;
                }
                
            }
        }

        /* Evento que es llamado desde el Sistema de combate que avisa que el ataque ya ha sido seleccionado */
        public void AtaqueElegido()
        {
            objetivosMaximos = GLOBAL.acciones[SistemaCombate.instance.jugador.accion].numObjetivos;
        }

        /* Metodo llamado cada vez que un toggle cambia su valor, dependiendo de si el toggle esta encendido o apagado
           aumentara o decrementara la cantidad de objetivos seleccionados, si se ha alcanzado el máximo número
           de objetivos seleccionables, se identifica a los objetivos seleccionados y se le pasa al gameObject 
           del luchador del jugador */
        void SeleccionaObjetivo(Toggle toggleObjetivo)
        {
            if (toggleObjetivo.isOn)
                objetivosSeleccionados++;
            else
                objetivosSeleccionados--;

            Debug.Log(objetivosSeleccionados);
            if (objetivosSeleccionados >= objetivosMaximos)
            {
                Debug.Log("Maximos Objetivos seleccionados");
                // Obtiene a los hijos del objeto asociado y analiza los valores de los toggle para saber
                // si se ha seleccionado a ese objetivo
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    if (gameObject.transform.GetChild(i).GetComponent<Toggle>().isOn) // Se comprueba el valor del toggle
                    {
                        // Si esta encendido el toggle se añade el luchador asociado como objetivo
                        // Se usa i+1 porque luchador[0] es el jugador
                        SistemaCombate.instance.jugador.objetivosSeleccionados.Add(SistemaCombate.luchadores[i+1]);
                    }
                }
                // Ejecuta el evento de FinDecision en el Sistema de combate
                ExecuteEvents.Execute<IMensajesCombate>(SistemaCombate.instance.gameObject, null,
                    (x, y) => { x.FinDecision(); });
            }
        }

        /* Metodo que se ejecuta cada vez que se deshabilita la UI de objetivos que borra todos los toggles*/
        private void OnDisable()
        {
            if (GLOBAL.enCombate)
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    Destroy(gameObject.transform.GetChild(i).gameObject);
                }
                objetivosSeleccionados = 0;
            }
        }
        
    }
}