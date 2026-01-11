using UnityEngine;
using UnityEngine.EventSystems;

namespace Codigo.Scripts
{
    public class AutoCameraOnSelect : BotonAutoSeleccionable , ISelectHandler
    {
        public int indice = 0;
        
        public void OnSelect(BaseEventData eventData)
        {
            SistemaCombate.instance.CambioEnfoqueCamara(SistemaCombate.luchadores[indice+1].transform, indice);
        }
    }
}