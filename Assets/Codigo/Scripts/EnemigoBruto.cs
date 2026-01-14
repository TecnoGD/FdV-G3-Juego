using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    public class EnemigoBruto : Luchador
    {
        public override int LuchadorIA(List<Luchador> luchadores)
        {
            objetivosSeleccionados.Clear();
            objetivosSeleccionados.Add(luchadores[0]); // Ataca al jugador

            List<int> indicesAtaquesFisicos = new List<int>();

            for (int i = 0; i < listaAcciones.Count; i++)
            {
                // Obtenemos la ID global de la acción
                int idAccion = listaAcciones[i];

                if (GLOBAL.acciones[idAccion].ObtenerTipo() == Ataque.FISICO)
                {
                    indicesAtaquesFisicos.Add(i);
                }
            }

            if (indicesAtaquesFisicos.Count > 0)
            {
                // Si tiene ataques físicos, elige uno al azar
                int aleatorio = Random.Range(0, indicesAtaquesFisicos.Count);
                return indicesAtaquesFisicos[aleatorio];
            }
            else
            {
                // Si no tiene ataques físicos configurados, usa cualquiera
                return Random.Range(0, listaAcciones.Count);
            }
        }
    }
}
