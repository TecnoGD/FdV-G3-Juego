using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class AccionMenuSlot : BotonAutoSeleccionable , ISelectHandler
    {
        public TMP_Text nombreAccion;
        public int indice;
        public UnityEvent<int> actualizarDatosAccion;


        public void OnSelect(BaseEventData eventData)
        {
            actualizarDatosAccion.Invoke(indice);
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