using System;
using UnityEngine;

namespace Codigo.Scripts
{
    [Serializable]
    public class ObjectSlot
    {
        public ObjetoConsumible objeto;
        public int cantidad;

        public ObjectSlot(ObjetoConsumible objeto, int cantidad)
        {
            this.objeto = objeto;
            this.cantidad = cantidad;
        }
    }
}