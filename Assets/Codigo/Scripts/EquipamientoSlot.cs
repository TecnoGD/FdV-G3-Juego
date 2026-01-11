using System;
using Codigo.Scripts.Sistema_Menu;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class EquipamientoSlot : BotonAutoSeleccionable, ISubmitHandler, ISelectHandler
    {
        public int index;
        public int lista;
        private string _descripcionTexto;
        public TMP_Text nombreTexto;
        public Image texturaTipoEquipamiento;
        public Image texturaEquipado;

        private void OnEnable()
        {
            Refresco();
        }

        public void Refresco()
        {
            var listaApropiada = GLOBAL.instance.ListasDeEquipamientos[lista];
            var listaApropiadaJugador = GLOBAL.instance.Jugador.ListasDeEquipamientosInventario[lista];
            
            if (index > -1 && index < listaApropiadaJugador.Count)
            {
                nombreTexto.text = listaApropiada[listaApropiadaJugador[index]].nombre;
                _descripcionTexto =  listaApropiada[listaApropiadaJugador[index]].descripcion;
                var color = texturaTipoEquipamiento.color;
                color.a = 1.0f;
                texturaTipoEquipamiento.color = color;
                texturaEquipado.gameObject.SetActive(
                    GLOBAL.instance.Jugador.equipamientoJugador[(int)MenuSelectorEquipamiento.equipamientoAModificar] ==
                    index);
            }
            else
            {
                nombreTexto.text = "Vacio";
                _descripcionTexto = "";
                var color = texturaTipoEquipamiento.color;
                color.a = 0.0f;
                texturaTipoEquipamiento.color = color;
                texturaEquipado.gameObject.SetActive(false);
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            GLOBAL.instance.Jugador.equipamientoJugador[(int)MenuSelectorEquipamiento.equipamientoAModificar] = index;
            GLOBAL.instance.Jugador.ActualizarEstadisticas();
            MenuSelectorEquipamiento.instance.refrescoEquipamiento.Invoke();
            NewMenuSystem.MenuAnterior();

        }

        public void OnSelect(BaseEventData eventData)
        {
            MenuSelectorEquipamiento.instance.descripcionSeleccion.text = _descripcionTexto;
            var tipoAModificar = (int)MenuSelectorEquipamiento.equipamientoAModificar;
            var listaApropiada = GLOBAL.instance.ListasDeEquipamientos[tipoAModificar];
            var listaApropiadaJugador = GLOBAL.instance.Jugador.ListasDeEquipamientosInventario[tipoAModificar];
            var modificadorNuevo = new int[5];
            if (index > -1 && index < listaApropiadaJugador.Count)
            {
                listaApropiada[listaApropiadaJugador[index]].modificadorEstadisticas.CopyTo(modificadorNuevo, 0);
            }
            
            var modificadorAntiguo = new int[5];
            if (GLOBAL.instance.Jugador.equipamientoJugador[tipoAModificar] > -1)
            {
                modificadorAntiguo =
                    listaApropiada[GLOBAL.instance.Jugador.equipamientoJugador[tipoAModificar]].modificadorEstadisticas;
            }
            
            
            MenuSelectorEquipamiento.instance.ActualizarComparadores(modificadorAntiguo ,modificadorNuevo);
            ScrollUpdate();
            
            
        }
        
        private void ScrollUpdate()
        {
            var scroll = GetComponentInParent<ScrollRect>();
            var target = gameObject.GetComponent<RectTransform>();
            var limiteSup = -scroll.viewport.rect.height;
            var limiteInf = 0;
            var current = target.localPosition.y + scroll.content.localPosition.y;

            if (scroll && !(current > limiteSup && current < limiteInf))
            {
                var vector3 = scroll.content.localPosition;
                vector3.x = 0;
                if(current > limiteSup)
                    vector3.y -= (target.rect.height);
                if(current < limiteInf)
                    vector3.y += (target.rect.height);
                scroll.content.localPosition = vector3;
            }
        }

        
    }
}