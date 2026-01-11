using System;
using System.Collections;
using System.Collections.Generic;
using Codigo.Scripts.Sistema_Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

namespace Codigo.Scripts
{
    public class MenuObjetivos : Menu
    {
        public GameObject prefabBotonObjetivo;  // Prefab del boton del objetivo
        public GameObject prefabBotonAtras;     // Prefab del boton para volver al menu anterior
        public int objetivosSeleccionados = 0;  // Número de objetivos seleccionados por el jugador
        public int objetivosMaximos = 0;        // Número máximo de objetivos que se pueden seleccionar
        public List<Luchador> listaObjetivos = new List<Luchador>();
        // Variables para el panel de análisis

        // Lista para guardar los elementos navegables (Toggles y Botones)
        private List<GameObject> listaNavegacion = new List<GameObject>();

        
        
        /* Metodo que se ejecuta cada vez que se habilita el menu de objetivos, el cual generara los toggle para
           seleccionar ls objetivos*/
        public void OnEnable()
        {
            if (GLOBAL.enCombate)
            {
                // limpiar por si acaso
                listaNavegacion.Clear();
                
                objetivosSeleccionados = 0;
                for (int i = 1; i < SistemaCombate.luchadores.Count; i++)
                {
                    // Inicializa el toggle del objetivo y obtiene su componente
                    GameObject toggle = Instantiate(prefabBotonObjetivo, contenedoresDeSeleccionables[0]);
                    Toggle toggleObjetivo = toggle.GetComponent<Toggle>();
                    toggle.GetComponent<AutoCameraOnSelect>().indice = i-1;
                    // Vincula el evento de onValueChanged del toggle a un metodo delegado que llama a otro metodo 
                    // que maneja la seleccion de objetivos
                    toggleObjetivo.onValueChanged.AddListener(delegate { SeleccionaObjetivo(toggleObjetivo); });
                    // Cambia el nombre del toggle por el del luchador asignado
                    toggleObjetivo.GetComponentInChildren<Text>().text = SistemaCombate.luchadores[i].nombre;
                    
                    // Añadimos el toggle a la lista de navegación
                    listaNavegacion.Add(toggle);
                }
                // añadimos el botón atrás a la lista de navegación tmb
                GameObject botonAtras = Instantiate(prefabBotonAtras, contenedoresDeSeleccionables[0]);
                listaNavegacion.Add(botonAtras);
                defaultElementFocus = listaNavegacion[0].GetComponent<Selectable>();
                
                // Llamamos a nuestro script que configura la navegación Automatica
                foreach (GameObject toggle in listaNavegacion)
                {
                    var navigation = toggle.gameObject.GetComponent<Selectable>().navigation;
                    navigation.mode = Navigation.Mode.Automatic;
                    toggle.gameObject.GetComponent<Selectable>().navigation = navigation;
                }
                //MenuNavegacionE.ConfigurarNavegacionVertical(listaNavegacion);
                
                // PONER EL FOCO (igual que en MenuAtaques)
                MenuNavegacionE.PonerFoco(listaNavegacion);
            }
        }

        /* Evento que es llamado desde el Sistema de combate que avisa que el ataque ya ha sido seleccionado */
        public void AtaqueElegido()
        {
            var luchCount = SistemaCombate.luchadores.Count - 1;
            var accion = GLOBAL.acciones[SistemaCombate.instance.jugador.accion];

            //Elimina el tener que seleccionar objetivo si solo hay 1 enemigo
            /*if (luchCount == 1)
            {
                listaObjetivos.Clear();
                listaObjetivos.Add(SistemaCombate.luchadores[1]);
                ExecuteEvents.Execute<IMensajesCombate>(SistemaCombate.instance.gameObject, null,
                    (x, y) => { x.FinDecision(listaObjetivos); });
                return;
            }*/

            if (accion.EstiloSeleccionObjetivo == Accion.TODOSENEMIGOS)
            {
                for (int i = 1; i < SistemaCombate.luchadores.Count; i++)
                {
                    listaObjetivos.Add(SistemaCombate.luchadores[i]);
                }
                ExecuteEvents.Execute<IMensajesCombate>(SistemaCombate.instance.gameObject, null,
                    (x, y) => { x.FinDecision(listaObjetivos); });
                return;
            }
            
            if(luchCount >= accion.numObjetivos)
                objetivosMaximos = accion.numObjetivos;
            else
                objetivosMaximos = luchCount;
        }
        
        public void Analisis()
        {
            objetivosMaximos = 1;
        }
        
        public void UsoObjeto(int tipoSeleccion)
        {
            switch (tipoSeleccion)
            {
                case ObjetoConsumible.SOLOENEMIGO:
                    objetivosMaximos = 1;
                    break;
                case ObjetoConsumible.TODOSENEMIGOS:
                    listaObjetivos.Clear();
                    for (int i = 1; i < SistemaCombate.luchadores.Count; i++)
                    {
                        listaObjetivos.Add(SistemaCombate.luchadores[i]);
                    }
                    ExecuteEvents.Execute<IMensajesCombate>(SistemaCombate.instance.gameObject, null,
                        (x, y) => { x.FinDecision(listaObjetivos); });
                    return;
                    
                case ObjetoConsumible.SOLOJUGADOR:
                    listaObjetivos.Clear();
                    listaObjetivos.Add(SistemaCombate.instance.jugador);
                    ExecuteEvents.Execute<IMensajesCombate>(SistemaCombate.instance.gameObject, null,
                        (x, y) => { x.FinDecision(listaObjetivos); });
                    break;
            }
        }

        

        /* Metodo llamado cada vez que un toggle cambia su valor, dependiendo de si el toggle esta encendido o apagado
           aumentara o decrementara la cantidad de objetivos seleccionados, si se ha alcanzado el máximo número
           de objetivos seleccionables, se identifica a los objetivos seleccionados y se le pasa al gameObject 
           del luchador del jugador */
        void SeleccionaObjetivo(Toggle toggleObjetivo)
        {
            listaObjetivos.Clear();
            if (toggleObjetivo.isOn)
                objetivosSeleccionados++;
            else
                objetivosSeleccionados--;

            //Debug.Log(objetivosSeleccionados);
            if (objetivosSeleccionados >= objetivosMaximos)
            {
                //Debug.Log("Maximos Objetivos seleccionados");
                // Obtiene a los hijos del objeto asociado y analiza los valores de los toggle para saber
                // si se ha seleccionado a ese objetivo
                // se resta -1 a la cantidad de hijos para evitar el boton de ir al anterior menu
                for (int i = 0; i < contenedoresDeSeleccionables[0].childCount-1; i++)
                {
                    if (contenedoresDeSeleccionables[0].GetChild(i).GetComponent<Toggle>().isOn) // Se comprueba el valor del toggle
                    {
                        // Si esta encendido el toggle se añade el luchador asociado como objetivo
                        // Se usa i+1 porque luchador[0] es el jugador
                        listaObjetivos.Add(SistemaCombate.luchadores[i + 1]);
                        //SistemaCombate.instance.jugador.objetivosSeleccionados.Add(SistemaCombate.luchadores[i+1]);
                    }
                }
                // Ejecuta el evento de FinDecision en el Sistema de combate
                ExecuteEvents.Execute<IMensajesCombate>(SistemaCombate.instance.gameObject, null,
                    (x, y) => { x.FinDecision(listaObjetivos); });
            }
        }

        /* Metodo que se ejecuta cada vez que se deshabilita la UI de objetivos que borra todos los toggles*/
        private void OnDisable()
        {
            if (GLOBAL.enCombate)
            {
                for (int i = 0; i < contenedoresDeSeleccionables[0]?.childCount; i++)
                {
                    Destroy(contenedoresDeSeleccionables[0].GetChild(i).gameObject);
                }
                objetivosSeleccionados = 0;
            }
        }

        public override void SalidaPorDefecto()
        {
            SistemaCombate.instance.CambioEnfoqueCamara();
        }
    }
}