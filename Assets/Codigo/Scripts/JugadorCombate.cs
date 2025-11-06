using UnityEngine;
using static Codigo.Scripts.DatosCombate;

namespace Codigo.Scripts
{
    public class JugadorCombate : Luchador
    {
        public override void InicioCombate()
        {
            Jugador jugador = gameObject.GetComponent<Jugador>();   // Obtiene los datos de combate de su componente jugador
            nombre = GLOBAL.guardado.nombre;
            estadisticas = jugador.estadisticasBase;
            listaAcciones  = jugador.accionesJugador;
            vida = estadisticas.vidaMax;
            animator = GetComponent<Animator>();
            objetivosSeleccionados.Clear();
            objetivosSeleccionados.TrimExcess();
            
        }
    }
}
