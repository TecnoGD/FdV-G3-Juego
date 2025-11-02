using Codigo.Scripts;
using UnityEngine;

public class MenuNavigator : MonoBehaviour
{
    
    public GameObject siguiente;    // atributo que contiene el menu a abrir
    public bool desactivaAnterior;  // atributo que define si el anterior menu se debe desactivar
    
    /* Metodo que llama al Sistema de menus para abrir el menu asignado como atributo*/
    public void CambioMenu()
    {
        if (!siguiente)
        {
            MenuSystem.MenuAnterior(); // Si el siguiente menu es null, el navegador vuelve al menu anterior
            
        }
        else
        {
            MenuSystem.SiguienteMenu(siguiente, desactivaAnterior); // Abre el siguiente menu y desactiva el anterior
                                                                    // si fuera indicado
        }
    }
}
