using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Codigo.Scripts
{
    public class ObjectSlotEquipadoMenu : BotonAutoSeleccionable, ISubmitHandler, ICancelHandler
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
        
        

        public void IniciaEquipar()
        {
            var lista = GLOBAL.instance.Jugador.objetosSeleccionadosCombate;
            var objetoAEquipar = GLOBAL.instance.objetoAEquipar;
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
            GLOBAL.instance.objetoAEquipar= null;
            gameObject.transform.parent.BroadcastMessage("Refresco");
            MenuSystem.GetMenuFocus().GetComponent<ObjectSlotMenu>().VueltaFocus();
            MenuSystem.MenuAnterior(true);
        }

        /*public void UsoObjeto()
        {
            if (objetoConsumible.objeto)
            {
                SistemaCombate.instance.UsoObjeto(index);
            }
        }*/
        public void OnSubmit(BaseEventData eventData)
        {
            IniciaEquipar();
        }

        public void OnCancel(BaseEventData eventData)
        {
            GLOBAL.instance.objetoAEquipar= null;
            gameObject.transform.parent.BroadcastMessage("Refresco");
            MenuSystem.GetMenuFocus().GetComponent<ObjectSlotMenu>().VueltaFocus();
            MenuSystem.MenuAnterior(true);
        }
    }
}