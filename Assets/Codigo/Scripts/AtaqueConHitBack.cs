using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "Accion", menuName = "Accion/AtaqueConHitBack")]
    
    public class AtaqueConHitBack : AtaqueConEfectoSecundario
    {
        public float hitBack = 0.0f;
        public override void EfectoSecundario(Luchador jugador, List<Luchador> objetivos)
        {
            Debug.Log((this.DañoBase*hitBack));
            var danio = jugador.CalcularDaño(jugador, (int)(this.DañoBase*hitBack), tipo);
            Debug.Log(danio);
            jugador.RecibeDaño(danio);
        }
    }
}