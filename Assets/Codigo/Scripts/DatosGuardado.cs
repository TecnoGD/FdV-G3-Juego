using System;
using System.Collections.Generic;
using UnityEngine;



namespace Codigo.Scripts
{
    [System.Serializable]
    public class DatosGuardado
    {
        public int id;                                          // Variable de prueba
        public string nombre;                                   // Nombre del jugador
        public DatosCombate.Estadisticas estadisticasJugador;   // Estadisticas base del jugador
        public int[] accionesJugador;                           // Lista de acciones del jugador
        [SerializeField] public List<DatosObjetoGuardado> objetosConsumibles;
        [NonSerialized] public List<ObjectSlot> objetosCargados;
        public int[] objetosSeleccionadosCombate;

        public int progresoHistoria; // Controla el avance de la historia (dialogo).
        public DatosGuardado(ObjetoConsumible obj)
        {
            estadisticasJugador = new DatosCombate.Estadisticas(50, 5, 1, 10, 1);
            id = 5;
            accionesJugador = new int[] {0,1,1,1,1};
            nombre = "Jugador"; 
            objetosConsumibles = new List<DatosObjetoGuardado>();
            objetosConsumibles.Add(new DatosObjetoGuardado(0, 1));
            objetosSeleccionadosCombate = new int[] {-1,-1,-1,-1};
            progresoHistoria = 0; // Empezamos desde 0 (inicio)
            
        }

        public void CargarObjetos()
        {
            
        }
    }

    [Serializable]
    public class DatosObjetoGuardado
    {
        public int id;
        public int cantidad;

        public DatosObjetoGuardado(int id, int cantidad)
        {
            this.id = id;
            this.cantidad = cantidad;
        }

    }
}