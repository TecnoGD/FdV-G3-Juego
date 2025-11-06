using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "Accion", menuName = "Accion/Ataque")]
    public class Ataque : Accion
    {
        public string Animacion;    // Animación del ataque
        public int DañoBase;        // Potencia base del atauqe
        public int tipo;
        public const int FISICO = 0, ESPECIAL = 1;

        public override void Ejecuta(Luchador self, Animator animator)
        {
            animator.Play(Animacion);   // Ejecuta la animación correspondiente al ataque
        }

        public override int ObtenerPotencia(int iteracion)
        {
            return DañoBase;            // Devuelve el daño base del ataque
        }

        public override int ObtenerTipo()
        {
            return tipo;
        }
    }
}