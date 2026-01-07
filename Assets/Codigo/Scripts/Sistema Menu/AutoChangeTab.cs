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
        
        
        public virtual void OnSelect(BaseEventData eventData)
        {
            cambiarTab.Invoke();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            entrarTab.Invoke();
        }
    }
}