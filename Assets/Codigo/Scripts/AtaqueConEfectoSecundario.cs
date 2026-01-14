using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    public class AtaqueConEfectoSecundario : Ataque
    {
        public float probabilidadEfectoSecundario = 0.0f;
        public override void Ejecuta(Luchador self, Animator animator)
        {
            base.Ejecuta(self, animator);
            
            
        }

        public void AplicarEfectoSecundario(Luchador jugador, List<Luchador> objetivos)
        {
            if(Random.Range(0.0f, 1.0f) < probabilidadEfectoSecundario) EfectoSecundario(jugador, objetivos);
        }

        public virtual void EfectoSecundario(Luchador jugador, List<Luchador> objetivos)
        {
            return;
        }
    }
}