using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    public class EnemigoGenerico : Luchador
    {
        
        public override int LuchadorIA(List<Luchador> luchadores)
        {
            objetivosSeleccionados.Add(luchadores[0]);
            
            return 0;
        }
        
    }
}