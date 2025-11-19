using System;
using System.Collections;
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
        // Variables para el panel de análisis
        public GameObject panelAnalisis;        // Panel que muestra la información del enemigo
        public TMP_Text textoNombreAnalisis;    // Texto que muestra el nombre del enemigo
        public TMP_Text textoVidaAnalisis;      // Texto que muestra la vida del enemigo
        public TMP_Text textoAtaqueAnalisis;    // Texto que muestra el ataque del enemigo
        public TMP_Text textoDefensaAnalisis;   // Texto que muestra la defensa del enemigo
        public TMP_Text textoAtaqueEspecialAnalisis;   // Nuevo hueco para Ataque Especial
        public TMP_Text textoDefensaEspecialAnalisis;  // Nuevo hueco para Defensa Especial
        public UnityEngine.UI.Button botonVolverAnalisis;  // Botón para volver atrás desde el análisis

        // Lista para guardar los elementos navegables (Toggles y Botones)
        private List<GameObject> listaNavegacion = new List<GameObject>();

        private bool modoAnalizar = false;  // Controla si estamos en modo analizar o en modo ataque


        void Start()
        {
            // Si el botón está asignado, le añadimos la función de cerrar
            if (botonVolverAnalisis != null)
            {
                botonVolverAnalisis.onClick.AddListener(CerrarAnalisis);
            }
        }
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
                    GameObject toggle = Instantiate(prefabBotonObjetivo, gameObject.transform);
                    Toggle toggleObjetivo = toggle.GetComponent<Toggle>();
                    // Vincula el evento de onValueChanged del toggle a un metodo delegado que llama a otro metodo 
                    // que maneja la seleccion de objetivos
                    toggleObjetivo.onValueChanged.AddListener(delegate { SeleccionaObjetivo(toggleObjetivo); });
                    // Cambia el nombre del toggle por el del luchador asignado
                    toggleObjetivo.GetComponentInChildren<Text>().text = SistemaCombate.luchadores[i].nombre;
                    
                    // Añadimos el toggle a la lista de navegación
                    listaNavegacion.Add(toggle);
                }
                // añadimos el botón atrás a la lista de navegación tmb
                GameObject botonAtras = Instantiate(prefabBotonAtras, gameObject.transform);
                botonAtras.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(VolverAtras); //Añadido para Analizar
                listaNavegacion.Add(botonAtras);
                
                // Llamamos a nuestro script que configura la navegación explícita
                MenuNavegacionE.ConfigurarNavegacionVertical(listaNavegacion);
                
                // PONER EL FOCO (igual que en MenuAtaques)
                MenuNavegacionE.PonerFoco(listaNavegacion);
            }
        }

        /* Evento que es llamado desde el Sistema de combate que avisa que el ataque ya ha sido seleccionado */
        public void AtaqueElegido()
        {
            gameObject.SetActive(true); //Añadido para Analizar
            objetivosMaximos = GLOBAL.acciones[SistemaCombate.instance.jugador.accion].numObjetivos;
        }

        public void AnalizarElegido()
        {
            gameObject.SetActive(true); // Activa el panel de seleccion de objetivos
            modoAnalizar = true;        // Marca que estamos en modo analizar
            MenuSystem.ResetMenuSystem(gameObject); // Reinicia el menu de seleccion de objetivos
        }

        /* Metodo llamado cada vez que un toggle cambia su valor, dependiendo de si el toggle esta encendido o apagado
           aumentara o decrementara la cantidad de objetivos seleccionados, si se ha alcanzado el máximo número
           de objetivos seleccionables, se identifica a los objetivos seleccionados y se le pasa al gameObject 
           del luchador del jugador */
        void SeleccionaObjetivo(Toggle toggleObjetivo)
        {
            // Si estamos en modo analizar, mostramos la información del enemigo
            if (modoAnalizar) //Este if es añadido para Analizar
            {
                if (toggleObjetivo.isOn)
                {
                    // Obtener el índice del toggle seleccionado
                    int indiceEnemigo = listaNavegacion.IndexOf(toggleObjetivo.gameObject);
                    // Mostrar la información del enemigo (lo haremos en el siguiente paso)
                    MostrarAnalisisEnemigo(indiceEnemigo + 1);  // +1 porque luchador[0] es el jugador
                    // Desactivar el toggle después de seleccionar
                    toggleObjetivo.isOn = false;
                }
                return;
            }
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
                // se resta -1 a la cantidad de hijos para evitar el boton de ir al anterior menu
                for (int i = 0; i < gameObject.transform.childCount-1; i++)
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
        private void OnDisable() //Código modificado para Analizar
    {
        if (GLOBAL.enCombate)
        {
            // SOLO borramos los botones que hayamos metido en la lista
            foreach (GameObject elemento in listaNavegacion)
            {
                if (elemento != null) Destroy(elemento);
            }
            listaNavegacion.Clear();
            objetivosSeleccionados = 0;
        }
    }

        /* Metodo que muestra la información del enemigo seleccionado en modo analizar
           PRE: indiceEnemigo -> int (índice del enemigo en la lista de luchadores)
           POST: Se muestra un panel con los atributos del enemigo seleccionado */
        void MostrarAnalisisEnemigo(int indiceEnemigo)
        {
            // Obtener el luchador enemigo
            Luchador enemigo = SistemaCombate.luchadores[indiceEnemigo];
        
            // Rellenar los textos con la información del enemigo
            textoNombreAnalisis.text = enemigo.nombre;
            textoVidaAnalisis.text = "Vida: " + enemigo.vida;
            textoAtaqueAnalisis.text = "Ataque: " + enemigo.estadisticas.ataque;
            textoDefensaAnalisis.text = "Defensa: " + enemigo.estadisticas.defensa;
            textoAtaqueEspecialAnalisis.text = "Atq. Esp: " + enemigo.estadisticas.ataqueEspecial;
            textoDefensaEspecialAnalisis.text = "Def. Esp: " + enemigo.estadisticas.defensaEspecial;
        
            // Mostrar el panel
            panelAnalisis.SetActive(true);
        
            // Ocultar el menú de objetivos mientras se ve el análisis
            gameObject.SetActive(false);

            // 3. NUEVO: Forzar la selección del botón "Volver"
            // Primero limpiamos la selección actual para evitar "fantasmas"
            EventSystem.current.SetSelectedGameObject(null);
            // Ahora le decimos al sistema de eventos que seleccione nuestro botón
            EventSystem.current.SetSelectedGameObject(botonVolverAnalisis.gameObject);
        }
        
        /* Metodo para cerrar el panel de análisis y volver a la selección de objetivos */
        void CerrarAnalisis()
        {
            panelAnalisis.SetActive(false); // Oculta los datos del enemigo
            gameObject.SetActive(true);     // Vuelve a mostrar la lista de objetivos (este mismo menú)
    
            // Opcional: Volvemos a poner el foco en la lista de navegación para usar mando/teclado
            if(listaNavegacion.Count > 0)
                MenuNavegacionE.PonerFoco(listaNavegacion);
        }

        /* Metodo para volver al menú de acciones (Cancelar selección) */
        void VolverAtras()
        {
            // 1. Limpieza de lógica
            modoAnalizar = false;
            SistemaCombate.instance.jugador.DecidirAccion(-1); 
        
            // 2. CORRECCIÓN: No uses 'gameObject.SetActive(false)' porque solo apaga el Content.
            // Debemos apagar el contenedor principal que abrió el sistema de combate.
            SistemaCombate.instance.ElementosUI[SistemaCombate.UIObjetivoCombate].SetActive(false);

            // 3. Obtenemos la referencia al menú al que queremos volver (UI Jugador)
            GameObject uiJugador = SistemaCombate.instance.ElementosUI[SistemaCombate.UIJugadorCombate];
        
            // 4. Activamos el menú visualmente
            uiJugador.SetActive(true);

            // 5. Le decimos al MenuSystem que reinicie el foco en ese menú
            MenuSystem.ResetMenuSystem(uiJugador);
        }
    }
}