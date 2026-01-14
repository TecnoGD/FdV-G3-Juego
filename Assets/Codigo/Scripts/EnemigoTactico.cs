using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    public class EnemigoTactico : Luchador
    {
        public override int LuchadorIA(List<Luchador> luchadores)
        {
            objetivosSeleccionados.Clear();
            Luchador jugador = luchadores[0];
            objetivosSeleccionados.Add(jugador);

            // Calculamos el porcentaje de vida del jugador
            float porcentajeVidaJugador = (float)jugador.vida / jugador.estadisticas.vidaMax;

            int tipoDeseado = -1;

            // SI EL JUGADOR ESTÁ DÉBIL, ATAQUE ESPECIAL
            if (porcentajeVidaJugador < 0.4f)
            {
                tipoDeseado = Ataque.ESPECIAL; 
            }
            // SI EL JUGADOR ESTÁ SANO, ATAQUE FÍSICO
            else
            {
                tipoDeseado = Ataque.FISICO;
            }

            // Buscamos los ataques que coincidan con el tipo deseado
            List<int> opcionesValidas = new List<int>();

            for (int i = 0; i < listaAcciones.Count; i++)
            {
                int idAccion = listaAcciones[i];
                if (GLOBAL.acciones[idAccion].ObtenerTipo() == tipoDeseado)
                {
                    opcionesValidas.Add(i);
                }
            }

            // Si encontramos ataques del tipo que queríamos, usamos uno
            if (opcionesValidas.Count > 0)
            {
                return opcionesValidas[Random.Range(0, opcionesValidas.Count)];
            }
            
            // Si no, tiro una aleatoria
            return Random.Range(0, listaAcciones.Count);
        }
    }
}
