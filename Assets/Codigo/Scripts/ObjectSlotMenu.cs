using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Codigo.Scripts
{
    public class ObjectSlotMenu : BotonAutoSeleccionable, ISelectHandler, ISubmitHandler
    {
        public int index;
        public bool esEsquipado;
        public TMP_Text texto;
        public TMP_Text cantidadTexto;
        public Image textura;
        public Image texturaEquipado;
        public ObjectSlot objetoConsumible;
        public MostrarDatosObjeto mostradorDatos;
        public GameObject ObjetosEquipadosSlots;
        

        void OnEnable()
        {
            if(index <  GLOBAL.instance.Jugador.listaObjetos.Count)
                objetoConsumible = GLOBAL.instance.Jugador.listaObjetos[index];
            
            if (objetoConsumible.objeto)
            {
                texto.text = objetoConsumible.objeto.nombre;
                var imagen = textura.sprite = objetoConsumible.objeto.textura;
                cantidadTexto.text = objetoConsumible.cantidad.ToString();
                var color = textura.color;
                color.a = 1.0f;
                textura.color = color;
                if (GLOBAL.instance.Jugador.objetosSeleccionadosCombate.Contains(objetoConsumible))
                {
                    var equipadoColor = texturaEquipado.color;
                    equipadoColor.a = 1.0f;
                    texturaEquipado.color = equipadoColor;
                }
                else
                {
                    var equipadoColor = texturaEquipado.color;
                    equipadoColor.a = 0.0f;
                    texturaEquipado.color = equipadoColor;
                }
            }else
            {
                cantidadTexto.text = "";
                texto.text = "Vacio";
                var color = textura.color;
                color.a = 0.0f;
                textura.color = color;
            }
            
            
        }

        /*public void UsoObjeto()
        {
            if (objetoConsumible.objeto)
            {
                SistemaCombate.instance.UsoObjeto(index);
            }
        }*/
        public void OnSelect(BaseEventData eventData)
        {
            mostradorDatos.CambiarDatos(objetoConsumible.objeto);
            var scroll = GetComponentInParent<ScrollRect>();
            var target = gameObject.GetComponent<RectTransform>();
            var limiteSup = -scroll.viewport.rect.height;
            var limiteInf = 0;
            var current = target.localPosition.y + scroll.content.localPosition.y;

            if (scroll && !(current > limiteSup && current < limiteInf))
            {
                var vector3 = scroll.viewport.localPosition;
                vector3.x = 0;
                vector3.y = 0 - (vector3.y + target.localPosition.y);
                if(current > limiteSup)
                    vector3.y += (scroll.viewport.rect.height / 2) - (target.rect.height / 2);
                if(current < limiteInf)
                    vector3.y -= (scroll.viewport.rect.height / 2) - (target.rect.height / 2);
                scroll.content.localPosition = vector3;
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (objetoConsumible.objeto)
            {
                var scroll = GetComponentInParent<ScrollRect>();
                foreach (var objetos in scroll.content.GetComponentsInChildren<Selectable>())
                {
                    objetos.interactable = false;
                }

                foreach (var slots in ObjetosEquipadosSlots.transform.GetComponentsInChildren<Selectable>())
                {
                    slots.interactable = true;
                }
                
                GLOBAL.instance.objetoAEquipar = objetoConsumible;
                MenuSystem.SiguienteMenu(gameObject);
                EventSystem.current.SetSelectedGameObject(ObjetosEquipadosSlots.transform.GetChild(0).gameObject);
            }
        }
        
        public void VueltaFocus(){
            var scroll = GetComponentInParent<ScrollRect>();
            foreach (var objetos in scroll.content.GetComponentsInChildren<Selectable>())
            {
                objetos.interactable = true;
            }

            foreach (var slots in ObjetosEquipadosSlots.transform.GetComponentsInChildren<Selectable>())
            {
                slots.interactable = false;
            }
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        
    }
}