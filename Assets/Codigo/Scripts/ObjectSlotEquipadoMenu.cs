using System;
using Codigo.Scripts.Sistema_Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Codigo.Scripts
{
    public class ObjectSlotEquipadoMenu : BotonAutoSeleccionable, ICancelHandler
    {
        public int index;
        public bool esEsquipado;
        public TMP_Text texto;
        public TMP_Text cantidadTexto;
        public Image textura;
        public ObjectSlot objetoConsumible;
        public UnityEvent finEleccion;

        void OnEnable()
        {
            Refresco();
            //gameObject.GetComponent<Selectable>().interactable = false;
        }
        
        void Refresco()
        {
            
            if(index <  GLOBAL.instance.Jugador.objetosSeleccionadosCombate.Length)
                objetoConsumible = GLOBAL.instance.Jugador.objetosSeleccionadosCombate[index];
            
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
        
        

        private void IniciaEquipar()
        {
            var lista = GLOBAL.instance.Jugador.objetosSeleccionadosCombate;
            var objetoAEquipar = MenuObjetos.Instancia.objetoSeleccionado.objetoConsumible;
            var terminado = false;
            for (var i = 0; i < lista.Length && !terminado; i++)
            {
                if (lista[i] == objetoAEquipar)
                {
                    lista[i] = new ObjectSlot(null, -1);;
                    terminado = true;
                }
            }

            lista[index] = objetoAEquipar;
            GLOBAL.instance.Jugador.objetosSeleccionadosCombate = lista;
            MenuObjetos.Instancia.contenedoresDeSeleccionables[0].BroadcastMessage("Refresco");
            gameObject.transform.parent.BroadcastMessage("Refresco");
            MenuObjetos.Instancia.objetoSeleccionado= null;
            NewMenuSystem.MenuAnterior();
            NewMenuSystem.MenuAnterior();
        }

        /*public void UsoObjeto()
        {
            if (objetoConsumible.objeto)
            {
                SistemaCombate.instance.UsoObjeto(index);
            }
        }*/
        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            IniciaEquipar();
        }

        public void OnCancel(BaseEventData eventData)
        {
            MenuObjetos.Instancia.objetoSeleccionado= null;
        }
    }
}