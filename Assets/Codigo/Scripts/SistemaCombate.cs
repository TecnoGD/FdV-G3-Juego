using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Codigo.Scripts
{
    public class SistemaCombate : MonoBehaviour , IMensajesCombate
    {
        public static SistemaCombate instance;
        public int turno = -1;                                          // -1 jugador esta decidiendo e IAs ejecutan
        public static List<Luchador> luchadores = new List<Luchador>(); // Lista de los luchadores activos en combate
        public Luchador jugador;                                        // Variable que contiene al GameObject del jugador
        public List<GameObject> UIGeneral;                              // Lista de elementos generales de la UI de combate (vida jugador, vida enemigos, texto de turno)
        public List<TMP_Text> TextoUI;                                  // Lista que contiene los textos de vida de los luchadores en combate
        public List<TMP_Text> TextoVidas = new List<TMP_Text>();
        public List<GameObject> ElementosUI;                            // Lista que contiene los elementos de UI de decision del jugador (Accion, ataque, objetivos...)
        public const int UICajaCombate = 0 ,UIJugadorCombate = 1, UIAccionesCombate = 2, UIObjetivoCombate = 3, UIVidaEnemigos = 4; // Utilizar estas constantes para mejor lectura del codigo a la hora de usar la variable "ElementosUI"
        public GameObject prefabVidaEnemigos;                           // Prefab de la UI de la vida de los enemigos

        // Inicializa lo necesario para el combate
        void Start()
        {
            instance = this;
            gameObject.SetActive(false);
        }
        public void IniciarCombate()
        {
            luchadores.Add(GameObject.FindGameObjectWithTag("Luchador Jugador").GetComponent<Luchador>());      // Busca al GameObject del jugador y lo añade a lista de luchadores
            GameObject[] e = GameObject.FindGameObjectsWithTag("Luchador Enemigo");                             // Busca los GameObjects de los enemigos y los añaden a lista de luchadores
                                                                                                                //
            foreach (GameObject g in e)                                                                         //
            {                                                                                                   //
                Luchador h = g.GetComponent<Luchador>();                                                        //
                luchadores.Add(h);                                                                              //
                h.InicioCombate();                                                                              // Inicializa al luchador
                // Temporal posible cambio (Instancia los paneles de vida de los enemigos)
                TextoVidas.Add(Instantiate(prefabVidaEnemigos, ElementosUI[UIVidaEnemigos].transform).transform.GetChild(0).GetComponent<TMP_Text>());
            }                                                                                                   //
            jugador = luchadores[0];                                                                            // Define la variable luchador al GameObject del jugador
            jugador.InicioCombate();                                                                            // Inicializa al jugador para el combate
            var pos = jugador.gameObject.transform.position;
            jugador.gameObject.transform.position =  new Vector3(-3.99f, pos.y, pos.z);              
                                                                                                                //
            gameObject.transform.BroadcastMessage("InicioCombate");                                  // El sistema de combate envia un mensaje de inicio de combate a todos sus hijos
            ElementosUI[UIAccionesCombate].gameObject.SetActive(false);                                         // Desactiva el menu de acciones de combate
            ElementosUI[UIObjetivoCombate].gameObject.SetActive(false);                                         // Desactiva el menu de seleccion de objetivos 
            MenuSystem.ResetMenuSystem(ElementosUI[UIJugadorCombate]);                                          // Reinicia el sistema de menu actual y lo inicializa con la UI de jugador 
            ElementosUI[UICajaCombate].SetActive(true);                                                         // Habilita la UI de la Caja de Combate
            HabilitarUICombate(true);                                                                     // Habilita el resto de elementos de UI de combate
            GLOBAL.enCombate = true;                                                                            // Marca la variable global de estado de en combate a verdadero
        }

        /*Dependiendo del parametro estado, activará o no los elementos generales de
         combate:
         PRE: estado -> bool
         POST: GameObjects: Turno, Vida, Vida Enemigo -> SetActive(estado)*/
        void HabilitarUICombate(bool estado)
        {
            foreach (GameObject g in UIGeneral)
            {
                g.SetActive(estado);
            }
        }

        /*Dependiendo del parametro estado, avanza al menu de eleccion de accion o reinicia el sistema de menus:
         PRE: estado -> bool
         POST: estado = true -> Se muestra el menu de eleccion de acciones
               estado = false -> Se reinicia el sistema de menus
        */
        void JugadorDecide(bool estado)
        {
            if(estado)
                MenuSystem.SiguienteMenu(ElementosUI[UIJugadorCombate]);
            else
                MenuSystem.ResetMenuSystem();
        }

        /* Metodo que escoje la accion elegida (que no sea ataque) por el jugador pasado por el parámetro accion 
           PRE: accion -> int [1,2]
           POST: accion = 1 -> El jugador defiende durante el turno
                 accion = 2 -> El jugador pasa el turno sin hacer nada
                 Después se desactiva la interfaz de decisión y se ejecuta las IAs del resto de luchadores
        */
        public void AccionEscogida(int accion)
        {
            switch (accion)
            {
                case 1:
                    luchadores[0].defiende = true;
                    break;
                case 2:
                    break;
            }
            FinDecision();                                      // Invoca el metodo que ejecuta las IAs del resto de lucahdores
        }

        /* Metodo que ejecuta las IAs de los luchadores que no son el jugador siendo invocadas desde su clase
           POST: Se ejecutan todas las funciones de IA de los luchadores no jugadores que deciden su ataque y objetivos
                 luego invoca al metodo que inicia la ejecución de los turnos de cada luchador
        */
        private void EnemigosDeciden()
        {
            for(int i = 1; i < luchadores.Count; i++)
            {
                luchadores[i].DecidirAccion(luchadores[i].LuchadorIA(luchadores));
            }
            EjecucionTurnos();
        }

        void Update()
        {
            TextoUI[0].text = "Vida: " + luchadores[0].vida;
            // Temporal, posible cambio
            for(int i = 1; i < luchadores.Count; i++)
                TextoVidas[i-1].text = "Vida E: " + luchadores[i].vida;
        }
        
        /* Metodo que ejecuta la accion del luchador al que le corresponde el turno actual
           POST: la variable "turno" incrementa en 1 (Inicialmente -1 al comienzo del primer turno)
                 turno < numero_de_luchares_en_combate -> Se invoca al metodo dentro del gameObject del luchador que ejecuta la accion escogida por dicho luchador
                 turno >= numero_de_luchares_en_combate -> Se invoca al metodo dentro del gameObject del luchador que resetea los valores necesarios para poder avanzar el combate
                                                           Se establece la variable turno a -1 (El jugador vuelve a poder decidir)
                                                           Se invoca al metodo que habilita el menu de decision del jugador 
        */
        private void EjecucionTurnos()
        {
            turno++;
            if (turno >= luchadores.Count)
            {
                foreach (var luchador in luchadores)
                {
                    luchador.ResetTurno();
                }
                turno = -1;
                JugadorDecide(true);
                return;
            }
            
            luchadores[turno].EjecutarAccion(luchadores);
        }

        /*IEnumerator TestEspera()
        {
            yield return null;
            yield return new WaitUntil(() => luchadores[turno].animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
            EjecucionTurnos();
        }*/

        /* Evento de la interfaz IMensajesCombate, llamada por la clase Luchador cuando termina de realizar la accion de su turno
           una vez llamada comprobara si hay luchadores que se hayan quedado sin vida, si es asi llama al metodo dentro de la clase
           Luchador que maneja su muerte y es borrado de la lista de luchadores
           
           Luego revisará si se cumple alguna de las condiciones que hagan que termine el combate, si no se cumple ninguna de esas 
           condiciones continuará con la ejecucion de los turnos
           
           POST: - vida_jugador -> Se llama al metodo que acaba el combate indicando que el jugador a perdido (FinDeCombate(false))
                 - numero_de_luchadores = 1 && vida_jugador > 0 -> Se llama al metodo que acaba el combate indicando que el jugador a ganado (FinDeCombate(true)) 
                 - ninguna de la anteriores -> Se continua con la ejecucion de los turnos */
        public void FinAccion()
        {
            for (int i = 0; i < luchadores.Count; i++)
            {
                if (luchadores[i].vida == 0)
                {
                    luchadores[i].LuchadorDerrotado();
                    luchadores.RemoveAt(i);
                    if (i > 0)
                    {
                        Destroy(TextoVidas[i-1].transform.parent.gameObject);   //Destruye los paneles de vida de los enemigos
                        TextoVidas.RemoveAt(i-1);
                    }
                    i--;
                }
            }
            
            if (jugador.vida == 0)
            {
                FinDeCombate(false);
                return;
            }

            if (luchadores.Count == 1 && jugador.vida > 0)
            {
                FinDeCombate(true);
                return;
            }
            
            EjecucionTurnos();
        }

        /* Evento de la interfaz de IMensajesCombate, llamada por la UI de Acciones una vez seleccionado
           el ataque que el jugador desea realizar enviada como parámetro (accion)
           POST: - Establece la accion elegida por el jugador al gameObject del jugador
                 - Envia un mensaje a la UI de objetivos y a todos sus hijos indicando que el ataque ya se escogió
                   para habilitar la UI de objetivos*/
        public void AtaqueElegido(int accion)
        {
            luchadores[0].DecidirAccion(accion);
            ElementosUI[UIObjetivoCombate].BroadcastMessage("AtaqueElegido");
        }

        /* Evento de la interfaz de IMensajesCombate, llamada por la UI de Objetivos una vez seleccionado
           todos los objetivos deseados para realizar el ataque 
           POST: desactiva la UI de decision y llama al metodo que ejecuta las IAs del resto de luchadores*/
        public void FinDecision()
        {
            JugadorDecide(false);
            EnemigosDeciden();
        }

        /* Metodo que finaliza el combate, cuyo unico parámetro de entrada decide si es una victorio o derrota
         POST: - Se envia un mensaje por consola: victorioso = true -> "Has ganado"
                                                  victorioso = false -> "Has perdido"
               - Se deshabilita la UI de combate
               - Se deshabilita el gameObject del sistema de combate
               - Se establece la variable global de enCombate a falso */
        private void FinDeCombate(bool victorioso)
        {
            Debug.Log(victorioso ? "Has Ganado" : "Has Perdido");
            HabilitarUICombate(false);
            luchadores.Clear();
            TextoVidas.Clear();
            gameObject.SetActive(false);
            GLOBAL.enCombate = false;
        }
        
    }
}