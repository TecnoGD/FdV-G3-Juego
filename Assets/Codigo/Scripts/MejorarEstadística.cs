using System;
using Codigo.Scripts.Sistema_Menu;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Codigo.Scripts
{
    public class MejorarEstadística : BotonAutoSeleccionable
    {
        public GameObject panel;
        public TMP_Text textoBoton;
        public Estadistica estadisticaAMejorar;
        public int cantidad;
        

        public void MejorarEstadistica()
        {
            GLOBAL.instance.Jugador.estadisticasBase.IncrementStat(estadisticaAMejorar, cantidad);
            GLOBAL.instance.Jugador.ActualizarEstadisticas();
            SistemaCombate.instance.CerrarCombate();
            Destroy(panel);
            
        }
    }
}