using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    public class EnemigoBerserker : Luchador
    {
        public override int LuchadorIA(List<Luchador> luchadores)
        {
            objetivosSeleccionados.Clear();
            objetivosSeleccionados.Add(luchadores[0]);

            // Calculamos MI propia vida
            float miPorcentajeVida = (float)vida / estadisticas.vidaMax;

            // SI ESTOY HERIDO, MODO FURIA
            if (miPorcentajeVida < 0.5f)
            {
                // Usar ataques Especiales
                List<int> especiales = new List<int>();
                for (int i = 0; i < listaAcciones.Count; i++)
                {
                    if (GLOBAL.acciones[listaAcciones[i]].ObtenerTipo() == Ataque.ESPECIAL)
                    {
                        especiales.Add(i);
                    }
                }

                if (especiales.Count > 0)
                {
                    return especiales[Random.Range(0, especiales.Count)];
                }
            }

            // ESTADO NORMAL o si no tiene especiales: Ataque completamente aleatorio
            return Random.Range(0, listaAcciones.Count);
        }
    }
}
