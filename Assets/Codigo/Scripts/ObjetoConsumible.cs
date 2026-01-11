using System;
using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [CreateAssetMenu(fileName = "ObjetoConsumible", menuName = "ObjetosConsumibles/Objeto")]
    [Serializable]
    public class ObjetoConsumible : ScriptableObject
    {
        public int id;
        public string nombre;
        public string descripcion;
        public int estiloSeleccionObjetivo;
        public int precio;
        public Sprite textura;
        public const int SOLOJUGADOR = 0, SOLOENEMIGO = 1, TODOSENEMIGOS = 2;

        public ObjetoConsumible(string nombre, string descripcion, int estiloSeleccion)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.estiloSeleccionObjetivo  = estiloSeleccion;
        }

        public virtual void Ejecutar(List<Luchador> objetivos)
        {
            return;
        }

    }
}