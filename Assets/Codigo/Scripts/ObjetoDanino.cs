using System;
using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "ObjetoConsumible", menuName = "ObjetosConsumibles/ObjetoDanino")]
    [Serializable]
    public class ObjetoDanino : ObjetoConsumible
    {
        public int valorDaño;

        // Constructor que coincide con la estructura de ObjetoCurativo
        public ObjetoDanino(string nombre, string descripcion, int estiloSeleccion, int valorDaño) : base(nombre, descripcion, estiloSeleccion)
        {
            this.valorDaño = valorDaño;
        }

        public override void Ejecutar(List<Luchador> objetivos)
        {
            foreach(Luchador objetivo in objetivos)
            {
                // Usamos RecibeDaño para que se apliquen las defensas y la lógica de muerte del Luchador
                objetivo.RecibeDaño(valorDaño);
            }
        }
    }
}