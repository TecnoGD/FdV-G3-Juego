using UnityEngine.EventSystems;

namespace Codigo.Scripts
{
    // Interfaz que define los eventos del sistema de combate
    public interface IMensajesCombate : IEventSystemHandler
    {
        /* Evento que se llama cuando un luchador termina de ejecutar su turno*/
        void FinAccion();
        
        /* Evento que se llama cuando el jugador elige la accion que va a ejecutar durante su turno */
        void AtaqueElegido(int accion);
        
        /* Evento que se llama cuando el jugador termina todas las decisiones que tiene que hacer*/
        void FinDecision();
    }
}