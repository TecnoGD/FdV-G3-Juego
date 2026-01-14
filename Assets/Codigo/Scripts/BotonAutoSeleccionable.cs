using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class BotonAutoSeleccionable : MonoBehaviour, IPointerEnterHandler, ISelectHandler, ISubmitHandler
    {
        public Selectable boton;

        private void Awake()
        {
            boton = GetComponent<Selectable>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(boton.interactable)
                boton.Select();
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            GLOBAL.instance.hoverMenuSonido.Play();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            GLOBAL.instance.clickMenuSonido.Play();
        }
    }
}