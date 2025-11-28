using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class VisorEstadistica : MonoBehaviour
    {
        public string nombre;
        public int statIndex;
        public TMP_Text nombreEstadistica;
        public Slider barra;
        public TMP_Text valor;

        void Start()
        {
            nombreEstadistica.text = nombre;
        }
        private void OnEnable()
        {
            var valorStat = GLOBAL.instance.Jugador.estadisticasBase.GetStat(statIndex);
            barra.value = valorStat / 500f;
            valor.text = valorStat.ToString();
        }
    }
}