using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "AtaqueCurativo", menuName = "Accion/AtaqueCurativo")]
    public class AtaqueCurativo : Ataque
    {
        [Header("Configuración de Curación")]
        public int cantidadCuracion; // Cantidad de vida que recupera el usuario

        public override void Ejecuta(Luchador self, Animator animator)
        {
            // 1. Ejecutamos la lógica base (iniciar la animación de ataque)
            base.Ejecuta(self, animator);

            // 2. Aplicamos la curación al usuario (self)
            self.vida += cantidadCuracion;

            // 3. Comprobamos que no supere la vida máxima
            if (self.vida > self.estadisticas.vidaMax)
            {
                self.vida = self.estadisticas.vidaMax;
            }
            
            Debug.Log($"{self.nombre} se ha curado {cantidadCuracion} puntos de vida al atacar.");
        }
    }
}