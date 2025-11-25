using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class ObjectSlotCombate : MonoBehaviour
    {
        public int index;
        public TMP_Text texto;
        public TMP_Text cantidadTexto;
        public Image textura;
        public ObjectSlot objetoConsumible;
        

        public void InicioCombate()
        {
            objetoConsumible = SistemaCombate.instance.jugador.objetosConsumibles[index];
            if (objetoConsumible.objeto)
            {
                texto.text = objetoConsumible.objeto.nombre;
                var imagen = textura.sprite = objetoConsumible.objeto.textura;
                cantidadTexto.text = objetoConsumible.cantidad.ToString();
                var color = textura.color;
                color.a = 1.0f;
                textura.color = color;
            }else
            {
                cantidadTexto.text = "";
                texto.text = "Vacio";
                var color = textura.color;
                color.a = 0.0f;
                textura.color = color;
            }
        }

        void OnEnable()
        {
            if (objetoConsumible.objeto)
            {
                texto.text = objetoConsumible.objeto.nombre;
                var imagen = textura.sprite = objetoConsumible.objeto.textura;
                cantidadTexto.text = objetoConsumible.cantidad.ToString();
                var color = textura.color;
                color.a = 1.0f;
                textura.color = color;
            }else
            {
                cantidadTexto.text = "";
                texto.text = "Vacio";
                var color = textura.color;
                color.a = 0.0f;
                textura.color = color;
            }
        }

        public void UsoObjeto()
        {
            if (objetoConsumible.objeto)
            {
                SistemaCombate.instance.UsoObjeto(index);
            }
                
                
        }
    }
}