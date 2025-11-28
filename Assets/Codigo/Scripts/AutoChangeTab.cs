using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Codigo.Scripts
{
    public class AutoChangeTab : BotonAutoSeleccionable , ISelectHandler
    {
        public UnityEvent cambiarTab;
        
        public void OnSelect(BaseEventData eventData)
        {
            cambiarTab.Invoke();
        }
    }
}