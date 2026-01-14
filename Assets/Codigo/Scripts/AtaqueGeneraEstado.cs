using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "Accion", menuName = "Accion/AtaqueGeneraEstado")]
    public class AtaqueGeneraEstado : AtaqueConEfectoSecundario
    {
        public EstadoAlterado estadoAlterado;
        public override void EfectoSecundario(Luchador jugador, List<Luchador> objetivos)
        {
            foreach (var objetivo in objetivos)
            {
                objetivo.AplicarEstado(estadoAlterado);
            }
        }
    }
}