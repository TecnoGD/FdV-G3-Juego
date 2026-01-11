using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class VisorEstadistica : MonoBehaviour
    {
        public string nombre;
        public Estadistica statIndex;
        public TMP_Text nombreEstadistica;
        public Slider barra;
        public TMP_Text valor;

        void Start()
        {
            nombreEstadistica.text = nombre;
        }
        private void OnEnable()
        {
            RefrescarEstadistica();
        }

        public void RefrescarEstadistica()
        {
            var valorStat = GLOBAL.instance.Jugador.estadisticasEfectivas.GetStat(statIndex);
            barra.value = valorStat / 500f;
            valor.text = valorStat.ToString();
        }
    }
}