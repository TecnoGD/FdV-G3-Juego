using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Codigo.Scripts.DatosCombate;
using Random = UnityEngine.Random;

namespace Codigo.Scripts
{
    public enum EstadoAlterado { Ninguno, Sangrado, Aturdimiento, Veneno }
    public class Luchador : MonoBehaviour
    {
        public int id;                                                          // id del luchador (actualmente sin uso)
        public string nombre;                                                   // Nombre del luchador
        //Texturas
        public int vida;                                                        // Vida actual del luchador en combate
        public int accion = -1;                                                 // Accion a ejecutar durante el turno del luchador (-1 indica que no hace nada)
        public bool defiende = false;                                           // Indica si se aplica la reduccion de daño por defensa
        public Estadisticas estadisticas;                                       // Estadisticas del luchador
        public Animator animator;                                               // Componente animator del gameObject del luchador
        public DatosLuchador datos;                                             // Datos guardados del enemigo necesarios a cargar para iniciar el combate
        public List<int> listaAcciones;                                         // Lista de acciones que puede realizar el luchador
        public List<Luchador> objetivosSeleccionados =  new List<Luchador>();   // Objetivos seleccionados al que el luchador va a realizar la accion
        public ObjectSlot[] objetosConsumibles;
        public int objetoSeleccionado = 0;
        public float potenciadorVeneno = 0.0625f;
        
        
        public List<EstadoAlterado> estadosAlterados = new List<EstadoAlterado>();

        public void AplicarEstado(EstadoAlterado estado)
        {
            if (!estadosAlterados.Contains(estado))
            {
                estadosAlterados.Add(estado);
                if (estado == EstadoAlterado.Veneno)
                    potenciadorVeneno = 0.0625f;
            }
        }

        public void QuitarEstado(EstadoAlterado estado)
        {
            if (estadosAlterados.Contains(estado))
            {
                estadosAlterados.Remove(estado);
            }
        }

        public bool TieneEstado(EstadoAlterado estado)
        {
            return estadosAlterados.Contains(estado);
        }

        public void ActualizarDatos()
        {
            nombre = datos.nombre;
            estadisticas = datos.GetEstadisticas();
            listaAcciones  = datos.GetAcciones();
            vida = estadisticas.vidaMax;
            animator = gameObject.GetComponent<Animator>();
            objetivosSeleccionados.Clear();
            objetivosSeleccionados.TrimExcess();
        }
        
        
        /* Metodo encargado de iniciar la ejecución de la accion seleccionada
           PRE: - objetivos -> Una lista de luchadores Rango: [0,inf]
           POST: accion = -1 -> El luchador no hace nada
                 numero_Objetivos <= 0 -> El luchador falla y no hace nada (Aun temporal)
                 Si nada de lo anterior -> Se inicia la ejecucion de la accion*/
        public void EjecutarAccion(List<Luchador> objetivos)
        {
            bool fallo = false;
            
            if (accion == -1)  //Sin accion seleccionada pasa turno
            {
                fallo = true;
            }
            
            if (!fallo && objetivosSeleccionados.Count <= 0)  //Sin objetivos en combate el ataque falla
            {
                fallo = true;
            }

            if (TieneEstado(EstadoAlterado.Aturdimiento) && Random.Range(0f, 1f) <= 0.1)
            {
                fallo = true;
            }
            
            if (fallo)
            {
                FinAccionLuchador();
                return;
            }

            if (accion < -1)
            {
                objetosConsumibles[objetoSeleccionado].objeto.Ejecutar(objetivosSeleccionados);
                objetosConsumibles[objetoSeleccionado].cantidad--;
                if (objetosConsumibles[objetoSeleccionado].cantidad <= 0)
                {
                    GLOBAL.instance.Jugador?.listaObjetos.Remove(objetosConsumibles[objetoSeleccionado]);
                    objetosConsumibles[objetoSeleccionado].objeto = null;
                    objetosConsumibles[objetoSeleccionado].cantidad = -1;
                }

                FinAccionLuchador();    //SOLO SI EL OBJETO NO TIENE ANIMACION
                return;
            }
               
            GLOBAL.acciones[accion].Ejecuta(this, animator);  // Obtiene los datos de la accion a 
                                                                                 // traves de la variable global "acciones"
        }
        
        /* Metodo que establece el atributo de este luchador a la accion pasada como parámetro
           PRE: - seleccion -> int Rango: [-1, numero_elementos_lista_de_acciones]*/
        public void DecidirAccion(int seleccion)
        {
            accion = seleccion;
        }

        /* Metodo llamado por los eventos de animacion del ataque que produce obtiene la potencia del ataque,
         calcula el daño final y lo aplica al objetivo seleccionado y tras terminar limpia la lista de objetivos*/
        public void ProducirDaño()
        {
            for (var i = 0; i < objetivosSeleccionados.Count; i++)
            {
                var danio = CalcularDaño(objetivosSeleccionados[i], GLOBAL.acciones[accion].ObtenerPotencia(i),
                    GLOBAL.acciones[accion].ObtenerTipo());
                objetivosSeleccionados[i].RecibeDaño(danio);
                if (accion >= 0 && GLOBAL.acciones[accion] as AtaqueConHitBack)
                {
                    ((AtaqueConHitBack)GLOBAL.acciones[accion]).EfectoSecundario(this,objetivosSeleccionados);
                }
            }
        }

        public int CalcularDaño(Luchador luchador, int potencia, int tipo)
        {
            float danio = 0;

            var estadisticaAtaque = tipo switch
            {
                Ataque.FISICO => estadisticas.ataque,
                Ataque.ESPECIAL => estadisticas.ataqueEspecial,
                _ => 0
            };

            var defensaObjetivo = tipo switch
            {
                Ataque.FISICO => luchador.estadisticas.defensa,
                Ataque.ESPECIAL => luchador.estadisticas.defensaEspecial,
                _ => 0
            };
            //objetivosSeleccionados[i].RecibeDaño((int)((acc.ObtenerPotencia(i)*estadisticaAtaque*0.5f)/(defensaObjetivo*10f)));
            //Debug.Log((int)((acc.ObtenerPotencia(i)*estadisticaAtaque*0.5f)/(defensaObjetivo*10f)));
            danio = Random.Range(0.9f, 1.1f) * (estadisticaAtaque * (100f / (100f + defensaObjetivo)));
            //Debug.Log((int)danio);
            return (int)danio;
        }
        
        /* Funcion que aplica el daño recibido aplicando distintos modificadores si fuera necesario y devuelve el daño
           final infligido
           POST: - defiende = true -> dañoRecibido = daño_recibido/2 
                 - dañoRecibido = 0 -> dañoRecibido = 1
                 - vida < dañoRecibido -> vida = 0 (si el daño recibido es mayor a la vida actual, la vida pasa directamente
                                                    a 0 para evitar valores negativos) */
        public int RecibeDaño(int dañoRecibido)
        {
            if (defiende)
                dañoRecibido /= 2;
        
            if (dañoRecibido == 0)
                dañoRecibido = 1;
        
            int dañoReal = vida;
        
            if (vida < dañoRecibido)
            {
                vida = 0;
            }
            else
                vida -= dañoRecibido;

            SistemaCombate.instance.MostrarDatosEnemigo(this);
            return dañoReal - vida;
        }

        /* Metodo llamado por la animacion del ataque que indica aue esta ha terminado, ejecutando otro evento para el
           sistema de combate que indica que el luchador a terminado su accion y, por lo tanto, su turno*/
        private void FinAccionLuchador()
        {
            if (TieneEstado(EstadoAlterado.Veneno))
            {
                var danoVen = (int)(estadisticas.vidaMax * potenciadorVeneno);
                potenciadorVeneno += 0.0625f;
                RecibeDaño(danoVen);
            }
            if (TieneEstado(EstadoAlterado.Sangrado))
            {
                var danoVen = (int)(estadisticas.vidaMax * 0.10);
                RecibeDaño(danoVen);
            }

            if (accion >= 0 && GLOBAL.acciones[accion] as AtaqueConEfectoSecundario)
            {
                ((AtaqueConEfectoSecundario)GLOBAL.acciones[accion]).AplicarEfectoSecundario(SistemaCombate.instance.jugador, objetivosSeleccionados);
            }
            
            ExecuteEvents.Execute<IMensajesCombate>(SistemaCombate.instance.gameObject, null,
                (x, y) => { x.FinAccion(); });
        }

        /* Metodo que reinicia los valores necesarios cuando acaba la ejecucion de turnos para una correcta ejecución de
           turnos futuros
           POST: - defiende = false (Se deja de defender)
                 - accion = -1 (la accion vuelve a su valor de inicio)*/
        public void ResetTurno()
        {
            defiende = false;
            if (accion != -1)
            {
                objetivosSeleccionados.Clear();
                objetivosSeleccionados.TrimExcess();
            }
            accion = -1;
        }

        /* Metodo llamado por el Sistema de combate cuando se derrota a un enemigo
           POST: Se destruye el gameObject del enemigo*/
        public void LuchadorDerrotado()
        {
            Destroy(this.gameObject);
        }
        
        /* Función virtual (se puede hacer override para cambiar su comportamiento) que determina la accion que el
           luchador(no jugador) va a realizar durante su turno, devuelve un número que equivale a una entrada de la 
           lista de acciones del luchador.
           PRE: - objetivos -> Una lista de luchadores Rango: [0,inf]
           POST: - valor de una entrada válida de la lista de acciones del luchador
                 - lista de objetivos actualizada */
        public virtual int LuchadorIA(List<Luchador> luchadores) { return -1; }
        
        /* Metodo virtual (se puede hacer override para cambiar su comportamiento) que inicializa todos los valores
           necesarios del luchador para el combate*/
        public virtual void InicioCombate() { 
            listaAcciones  = datos.GetAcciones();
            estadisticas = datos.GetEstadisticas();
            nombre = datos.nombre;
            vida = estadisticas.vidaMax;
            animator = gameObject.GetComponent<Animator>();
            objetivosSeleccionados.Clear();
            objetivosSeleccionados.TrimExcess();
            
        }
    }
}