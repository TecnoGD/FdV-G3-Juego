using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Codigo.Scripts.Sistema_Menu
{
    public class RecordatorioTempo: MonoBehaviour
    {
        public float valorIncial = 4f;
        public float temporizador;
        public TMP_Text texto;

        void Start()
        {
            temporizador = valorIncial;
            texto.alpha = 0;
            InputSystem.onEvent.Call(eventPtr =>
            {
                temporizador = valorIncial;
                texto.alpha = 0;
            });
        }

        void Update()
        {
            if(temporizador >= 0 && !NewMenuSystem.DentroDeUnMenu() && !GLOBAL.EnEvento) temporizador -= Time.deltaTime;
            if (temporizador <= 0)
            {
                texto.alpha = 1;
            }
        }
    }
}