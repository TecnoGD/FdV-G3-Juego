using UnityEngine;
using UnityEngine.EventSystems;

namespace Codigo.Scripts
{
    public class AutoCameraOnSelect : BotonAutoSeleccionable
    {
        public int indice = 0;
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SistemaCombate.instance.CambioEnfoqueCamara(SistemaCombate.luchadores[indice+1].transform, indice);
        }
    }
}