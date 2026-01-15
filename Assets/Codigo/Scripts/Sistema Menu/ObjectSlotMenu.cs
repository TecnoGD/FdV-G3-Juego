using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace Codigo.Scripts.Sistema_Menu
{
    public class ObjectSlotMenu : BotonAutoSeleccionable
    {
        public int index;
        public TMP_Text texto;
        public TMP_Text cantidadTexto;
        public Image textura;
        public Image texturaEquipado;
        public ObjectSlot objetoConsumible;
        
        
        

        void OnEnable()
        {
            Refresco();
        }

        public void Refresco()
        {
            if(index <  GLOBAL.instance.Jugador.listaObjetos.Count)
                objetoConsumible = GLOBAL.instance.Jugador.listaObjetos[index];
            
            if (objetoConsumible.objeto)
            {
                texto.text = objetoConsumible.objeto.nombre;
                var imagen = textura.sprite = objetoConsumible.objeto.textura;
                cantidadTexto.text = "x" + objetoConsumible.cantidad.ToString();
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
                gameObject.SetActive(false);
            }
        }

        /*public void UsoObjeto()
        {
            if (objetoConsumible.objeto)
            {
                SistemaCombate.instance.UsoObjeto(index);
            }
        }*/
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            MenuObjetos.Instancia.mostradorDatos.CambiarDatos(objetoConsumible.objeto);
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
                    vector3.y += (scroll.viewport.rect.height / 2) - (target.rect.height / 2) - 10;
                if(current < limiteInf)
                    vector3.y -= (scroll.viewport.rect.height / 2) - (target.rect.height / 2) - 10;
                scroll.content.localPosition = vector3;
            }
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            if (!objetoConsumible.objeto) return;
            MenuObjetos.Instancia.objetoSeleccionado = this;
            NewMenuSystem.SiguienteMenu(MenuObjetos.Instancia.contextMenuObjetos);

        }
        
    }
}