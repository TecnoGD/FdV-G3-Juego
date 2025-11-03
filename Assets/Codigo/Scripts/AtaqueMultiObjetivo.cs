using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "Accion", menuName = "Accion/AtaqueMultiObjetivo")]
    public class AtaqueMultiObjetivo : Accion
    {
        public string Animacion;            // Animación del ataque
        public int DañoBase;                // Potencia base del atauqe
        public int maxObjetivos;            // Máximo número de objetivos seleccionables
        public float[] multiplicadores;     // Multiplicadores de daño

        public override void Ejecuta(Luchador self, Animator animator)
        {
            animator.Play(Animacion);       // Ejecuta la animación correspondiente al ataque
        }

        public override int ObtenerPotencia(int iteracion)
        {
            return (int)(DañoBase*multiplicadores[iteracion]); // Daño multiplicado por el multiplicador dependiendo de "iteracion"
        }
    }
}