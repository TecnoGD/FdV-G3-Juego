using System.Collections.Generic;
using UnityEngine;
using static Codigo.Scripts.DatosCombate;

namespace Codigo.Scripts
{
    
    // ScriptableObject que contiene los datos de un luchador
    [CreateAssetMenu(fileName = "DatosLuchador", menuName = "Luchador/DatosLuchador")]
    public class DatosLuchador : ScriptableObject
    {
        public int tipoLuchador;                    // Tipo de luchador
        public const int JUGADOR = 1, ENEMIGO = 2;  // Constantes para la variable tipoLuchador
        public string nombre;                       // Nombre del luchador
        public int VidaMax;                         // Vida maxima del luchador
        public int Ataque;                          // Valor de ataque del luchador
        public int Defensa;                         // Valor de defensa del luchador
        public int AtaqueEspecial;                  // Valor de ataque especial del luchador
        public int DefensaEspecial;                 // Valor de defensa especial del luchador
        public int dinero;
        public List<int> acciones;                  // Lista de acciones del luchador

        public Estadisticas GetEstadisticas()
        {
            return new Estadisticas(VidaMax, Ataque, Defensa, AtaqueEspecial, DefensaEspecial);
        }
        
        public List<int> GetAcciones()
        {
            return acciones;
        }
    }
}