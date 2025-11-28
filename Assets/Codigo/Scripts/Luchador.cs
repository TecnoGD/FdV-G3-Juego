using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Codigo.Scripts.DatosCombate;
using Random = UnityEngine.Random;

namespace Codigo.Scripts
{
    public class Luchador : MonoBehaviour
    {
        public int id;
        public string nombre;
        public int vida;
        public int accion = -1; // -1: Nada, -2: Objeto, >=0: Ataque
        public bool defiende = false;
        public Estadisticas estadisticas;
        public Animator animator;
        public DatosLuchador datos;
        public List<int> listaAcciones;
        public List<Luchador> objetivosSeleccionados = new List<Luchador>();
        public ObjectSlot[] objetosConsumibles;
        public int objetoSeleccionado = 0;
        
        /* Método encargado de iniciar la ejecución de la acción seleccionada */
        public void EjecutarAccion(List<Luchador> objetivos)
        {
            bool fallo = false;
            
            if (accion == -1) fallo = true;
            if (!fallo && objetivosSeleccionados.Count <= 0) fallo = true;
            
            if (fallo)
            {
                FinAccionLuchador();
                return;
            }

            // --- LÓGICA DE USO DE OBJETOS (Corrección del error de índice) ---
            if (accion < -1)
            {
                if (objetosConsumibles != null && objetoSeleccionado < objetosConsumibles.Length && objetosConsumibles[objetoSeleccionado].objeto != null)
                {
                    objetosConsumibles[objetoSeleccionado].objeto.Ejecutar(objetivosSeleccionados);
                    objetosConsumibles[objetoSeleccionado].cantidad--;
                    
                    if (objetosConsumibles[objetoSeleccionado].cantidad <= 0)
                    {
                        objetosConsumibles[objetoSeleccionado].objeto = null;
                        objetosConsumibles[objetoSeleccionado].cantidad = -1;
                    }
                }
                
                // IMPORTANTE: Limpiamos la lista para evitar bugs de curación doble
                objetivosSeleccionados.Clear();

                FinAccionLuchador(); 
                return; // Salimos aquí para no ejecutar GLOBAL.acciones con índice negativo
            }
            // ------------------------------------------------------------------
               
            // Ejecutar ataque normal (Solo llega aquí si accion >= 0)
            if (listaAcciones != null && accion < listaAcciones.Count)
            {
                GLOBAL.acciones[listaAcciones[accion]].Ejecuta(this, animator);  
            }
            else
            {
                Debug.LogError("Error: Intento de ejecutar acción fuera de rango. Accion: " + accion);
                FinAccionLuchador();
            }
        }
        
        public void DecidirAccion(int seleccion)
        {
            accion = seleccion;
        }

        /* Método llamado por la animación (o forzado) para calcular daño/curación */
        public void ProducirDaño()
        {
            if (accion < 0 || accion >= listaAcciones.Count) return; // Seguridad extra

            var acc = GLOBAL.acciones[listaAcciones[accion]];
            var tipo = acc.ObtenerTipo();
            int potenciaAtaque = acc.ObtenerPotencia(0); 

            // --- LÓGICA DE ATAQUE CURATIVO / HÍBRIDO ---
            if (tipo == 2) // 2 = CURATIVO
            {
                // 1. Curarse a sí mismo (5 puntos fijos)
                this.RecibeCuracion(5); 

                // 2. Hacer daño a TODOS los objetivos seleccionados (2 puntos fijos)
                for (int i = 0; i < objetivosSeleccionados.Count; i++)
                {
                    objetivosSeleccionados[i].RecibeDaño(2);
                    Debug.Log("Ataque híbrido: Daño realizado a " + objetivosSeleccionados[i].nombre);
                }

                objetivosSeleccionados.Clear();
                return; 
            }

            // --- LÓGICA DE ATAQUE FÍSICO / ESPECIAL ---
            var estadisticaAtaque = 0;
            switch (tipo)
            {
                case Ataque.FISICO:
                    estadisticaAtaque = estadisticas.ataque;
                    break;
                case Ataque.ESPECIAL:
                    estadisticaAtaque = estadisticas.ataqueEspecial;
                    break;
            }

            for (int i = 0; i < objetivosSeleccionados.Count; i++)
            {
                var defensaObjetivo = 0;
                switch (tipo)
                {
                    case Ataque.FISICO:
                        defensaObjetivo = objetivosSeleccionados[i].estadisticas.defensa;
                        break;
                    case Ataque.ESPECIAL:
                        defensaObjetivo = objetivosSeleccionados[i].estadisticas.defensaEspecial;
                        break;
                }

                // Fórmula: (Tu Fuerza + Potencia del Arma) * Reducción Defensa
                float danioBase = estadisticaAtaque + potenciaAtaque; 
                float factorDefensa = 100f / (100f + defensaObjetivo);
                float danio = Random.Range(0.9f, 1.1f) * (danioBase * factorDefensa);
                
                if (danio < 1) danio = 1;

                Debug.Log($"Daño calculado: {(int)danio}");
                objetivosSeleccionados[i].RecibeDaño((int)danio);
            }
            
            objetivosSeleccionados.Clear();
            objetivosSeleccionados.TrimExcess();
        }
        
        public int RecibeDaño(int dañoRecibido)
        {
            if (defiende) dañoRecibido /= 2;
            if (dañoRecibido == 0) dañoRecibido = 1;
        
            int dañoReal = vida;
        
            if (vida < dañoRecibido) vida = 0;
            else vida -= dañoRecibido;

            return dañoReal - vida;
        }

        public void RecibeCuracion(int cantidad)
        {
            vida += cantidad;
            if (vida > estadisticas.vidaMax)
            {
                vida = estadisticas.vidaMax;
            }
            Debug.Log(nombre + " se ha curado " + cantidad + " puntos de vida.");
        }

        private void FinAccionLuchador()
        {
            ExecuteEvents.Execute<IMensajesCombate>(SistemaCombate.instance.gameObject, null,
                (x, y) => { x.FinAccion(); });
        }

        public void ResetTurno()
        {
            defiende = false;
            if (accion != -1)
            {
                objetivosSeleccionados.Clear(); 
            }
            accion = -1;
        }

        public void LuchadorDerrotado()
        {
            Destroy(this.gameObject);
        }
        
        public virtual int LuchadorIA(List<Luchador> luchadores) { return -1; }
        
        public virtual void InicioCombate() 
        { 
            if (datos != null)
            {
                listaAcciones  = datos.GetAcciones();
                estadisticas = datos.GetEstadisticas();
                nombre = datos.nombre;
                vida = estadisticas.vidaMax;
            }
            
            animator = GetComponent<Animator>();
            objetivosSeleccionados.Clear();
        }
    }
}