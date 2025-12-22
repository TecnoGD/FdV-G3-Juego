using System;
using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "ObjetoCuraEstado", menuName = "ObjetosConsumibles/ObjetoCuraEstado")]
    [Serializable]
    public class ObjetoCuraEstado : ObjetoConsumible
    {
        public Luchador.EstadoAlterado estadoACurar;

        public ObjetoCuraEstado(string nombre, string descripcion, int estiloSeleccion, Luchador.EstadoAlterado estadoACurar) : base(nombre, descripcion, estiloSeleccion)
        {
            this.estadoACurar = estadoACurar;
        }

        public override void Ejecutar(List<Luchador> objetivos)
        {
            foreach(Luchador objetivo in objetivos)
            {
                objetivo.QuitarEstado(estadoACurar);
            }
        }

    }
}
