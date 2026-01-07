using UnityEngine;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public enum TipoEquipamiento
    {
        Arma,
        Armadura,
        Zapato,
        Accesorio
    }
    [CreateAssetMenu(fileName = "Equipamiento", menuName = "Equipamiento/Pieza de Equipamiento")]
    public class Equipamiento : ScriptableObject
    {
        public string nombre;
        public string descripcion;
        public TipoEquipamiento tipoEquipamiento;
        public int[] modificadorEstadisticas = {0,0,0,0,0};

        public virtual void Efecto()
        {
            return;
        }
    }
}