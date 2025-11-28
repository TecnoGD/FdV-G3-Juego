using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "NuevoEquipo", menuName = "Inventario/ObjetoEquipable")]
    public class ObjetoEquipable : ScriptableObject
    {
        public string nombre;
        [TextArea] public string descripcion;
        public Sprite icono;

        // Definimos si es arma o armadura para saber en qué hueco va
        public enum TipoEquipo { ARMA, ARMADURA }
        public TipoEquipo tipo;

        // Bonificadores (Sumarán a tus stats base)
        public int bonoAtaque;
        public int bonoDefensa;
        public int bonoAtaqueEspecial;
        public int bonoDefensaEspecial;
        public int bonoVida;
    }
}