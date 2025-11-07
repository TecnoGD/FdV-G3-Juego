using System;
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

        public DatosGuardado()
        {
            estadisticasJugador = new DatosCombate.Estadisticas(50, 5, 1, 10, 1);
            id = 5;
            accionesJugador = new int[] {0,1};
            nombre = "Jugador"; 
        }
    }
}