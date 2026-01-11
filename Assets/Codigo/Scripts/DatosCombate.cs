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

            public int GetStat(Estadistica stat)
            {
                return stat switch
                {
                    Estadistica.VidaMax => vidaMax,
                    Estadistica.Ataque => ataque,
                    Estadistica.Defensa => defensa,
                    Estadistica.AtaqueEspecial => ataqueEspecial,
                    Estadistica.DefensaEspecial => defensaEspecial,
                    _ => -1
                };
            }
            
            public void SetStat(Estadistica stat, int valor)
            {
                switch (stat)
                {
                    case Estadistica.VidaMax:
                        vidaMax = valor;
                        break;
                    case Estadistica.Ataque:
                        ataque = valor;
                        break;
                    case Estadistica.Defensa:
                        defensa = valor;
                        break;
                    case Estadistica.AtaqueEspecial:
                        ataqueEspecial = valor;
                        break;
                    case Estadistica.DefensaEspecial:
                        defensaEspecial = valor;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
                }
            }
            
            public void IncrementStat(Estadistica stat, int valor)
            {
                switch (stat)
                {
                    case Estadistica.VidaMax:
                        vidaMax += valor;
                        break;
                    case Estadistica.Ataque:
                        ataque += valor;
                        break;
                    case Estadistica.Defensa:
                        defensa += valor;
                        break;
                    case Estadistica.AtaqueEspecial:
                        ataqueEspecial += valor;
                        break;
                    case Estadistica.DefensaEspecial:
                        defensaEspecial += valor;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
                }
            }
        }
    }

    public enum Estadistica
    {
        VidaMax,
        Ataque,
        Defensa,
        AtaqueEspecial,
        DefensaEspecial
    }
    
}