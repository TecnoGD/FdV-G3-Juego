using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class BotonAutoSeleccionable : MonoBehaviour, IPointerEnterHandler
    {
        public Selectable boton;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(boton.interactable)
                boton.Select();
        }
    }
}