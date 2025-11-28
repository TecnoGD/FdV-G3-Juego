using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Codigo.Scripts
{
    // Clase que contiene los distintos tipos de datos para los combates
    public class DatosCombate
    {
        [Serializable]
        public struct Estadisticas
        {
            [FormerlySerializedAs("VidaMax")] public int vidaMax;
            [FormerlySerializedAs("Ataque")] public int ataque;
            [FormerlySerializedAs("Defensa")] public int defensa;
            [FormerlySerializedAs("AtaqueEspecial")] public int ataqueEspecial;
            [FormerlySerializedAs("DefensaEspecial")] public int defensaEspecial; 

            public Estadisticas(bool unitario)
            {
                vidaMax = 1;
                ataque = 1;
                defensa = 1;
                ataqueEspecial = 1;
                defensaEspecial = 1;
            }
            
            public Estadisticas(int vidaMaxP, int ataqueP, int defensaP, int ataqueEspecialP, int defensaEspecialP)
            {
                vidaMax = vidaMaxP;
                ataque = ataqueP;
                defensa = defensaP;
                ataqueEspecial = ataqueEspecialP;
                defensaEspecial = defensaEspecialP;
            }
            
            public Estadisticas(Estadisticas es)
            {
                vidaMax = es.vidaMax;
                ataque = es.ataque;
                defensa = es.defensa;
                ataqueEspecial = es.ataqueEspecial;
                defensaEspecial = es.defensaEspecial;
            }

            public int GetStat(int stat)
            {
                return stat switch
                {
                    0 => vidaMax,
                    1 => ataque,
                    2 => defensa,
                    3 => ataqueEspecial,
                    4 => defensaEspecial,
                    _ => -1
                };
            }
        }
    }
}