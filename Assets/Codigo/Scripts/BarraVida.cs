using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class BarraVida : MonoBehaviour
    {
        public Slider barra;
        public TMP_Text valor;
        
        private void OnEnable()
        {
            RefrescarEstadistica();
        }

        public void RefrescarEstadistica()
        {
            var valorStat = GLOBAL.instance.Jugador.estadisticasEfectivas.GetStat(Estadistica.VidaMax);
            barra.maxValue = valorStat;
            barra.value =  GLOBAL.instance.Jugador.vida;
            valor.text = GLOBAL.instance.Jugador.vida + "/" + valorStat;
        }
    }
}