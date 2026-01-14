using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class AutoChangeTab : BotonAutoSeleccionable , ISelectHandler, ISubmitHandler
    {
        public UnityEvent cambiarTab;
        public UnityEvent entrarTab;
        
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            cambiarTab.Invoke();
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            entrarTab.Invoke();
        }
    }
}