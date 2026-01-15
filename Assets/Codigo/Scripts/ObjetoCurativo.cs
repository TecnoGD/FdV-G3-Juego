using System;
using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "ObjetoConsumible", menuName = "ObjetosConsumibles/ObjetoCurativo")]
    [Serializable]
    public class ObjetoCurativo : ObjetoConsumible
    {
        public int valorCurativo;

        public ObjetoCurativo(string nombre, string descripcion, int estiloSeleccion, int valorCurativo) : base(nombre, descripcion, estiloSeleccion)
        {
            this.valorCurativo = valorCurativo;
        }

        public override void Ejecutar(List<Luchador> objetivos)
        {
            foreach (Luchador objetivo in objetivos)
            {
                objetivo.vida +=  valorCurativo; 
                if(objetivo.vida > objetivo.estadisticas.vidaMax)
                    objetivo.vida = objetivo.estadisticas.vidaMax;
            }
                
        }

        public void UsoFueraCombate(Jugador jugador)
        {
            jugador.vida += valorCurativo;
            if(jugador.vida > jugador.estadisticasEfectivas.vidaMax)
                jugador.vida = jugador.estadisticasEfectivas.vidaMax;
        }

    }
}