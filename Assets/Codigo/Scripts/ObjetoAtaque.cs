using System;
using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "ObjetoAtaque", menuName = "ObjetosConsumibles/ObjetoAtaque")]
    [Serializable]
    public class ObjetoAtaque : ObjetoConsumible
    {
        public int danoFijo; // Daño exacto que hará el objeto

        // Constructor necesario para inicializar
        public ObjetoAtaque(string nombre, string descripcion, int estiloSeleccion, int dano) : base(nombre, descripcion, estiloSeleccion)
        {
            this.danoFijo = dano;
        }

        public override void Ejecutar(List<Luchador> objetivos)
        {
            foreach(Luchador objetivo in objetivos)
            {
                // Hacemos daño directo (RecibeDaño ya resta la vida y devuelve cuánto restó)
                int danoRealizado = objetivo.RecibeDaño(danoFijo);
                Debug.Log(objetivo.nombre + " recibe " + danoRealizado + " de daño por " + nombre);
            }
        }
    }
}