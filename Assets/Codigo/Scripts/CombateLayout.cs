using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "CombateLayout", menuName = "CombateLayout/Layout")]
    public class CombateLayout : ScriptableObject
    {
        public List<DatosLuchador> luchadores;
    }
}