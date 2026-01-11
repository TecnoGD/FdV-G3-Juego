using System;
using TMPro;

namespace Codigo.Scripts.Sistema_Menu
{
    public class MenuDetallesJugador : Menu
    {
        public TMP_Text nombreJugador;
        public TMP_Text dineroJugador;


        private void Awake()
        {
            nombreJugador.text = "Nombre: " + GLOBAL.guardado.nombre;
        }

        private void OnEnable()
        {
            dineroJugador.text = "Dinero: " + GLOBAL.instance.Jugador.dinero + "$";
        }
    }
}