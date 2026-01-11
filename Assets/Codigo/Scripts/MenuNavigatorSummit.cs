using Codigo.Scripts.Sistema_Menu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Codigo.Scripts
{
    public class MenuNavigatorSummit : MonoBehaviour, ISubmitHandler
    {
        public Menu siguiente;    // atributo que contiene el menu a abrir
    
        /* Metodo que llama al Sistema de menus para abrir el menu asignado como atributo*/
        public void CambioMenu()
        {
            if (!siguiente)
            {
                NewMenuSystem.MenuAnterior();  // Si el siguiente menu es null, el navegador vuelve al menu anterior
                //MenuSystem.MenuAnterior(); 
            }
            else
            {
                NewMenuSystem.SiguienteMenu(siguiente);     // Abre el siguiente menu y desactiva el anterior
                // si fuera indicado
                //MenuSystem.SiguienteMenu(siguiente, desactivaAnterior); 
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            CambioMenu();
        }
    }
}