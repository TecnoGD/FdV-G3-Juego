using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class MenuSystem : MonoBehaviour
    {
        public static MenuSystem instance;                                      // instancia del objeto del sistema de menu
        private static Stack<GameObject> menuStack = new Stack<GameObject>();   // Pila de menus 
        private static GameObject menuFocus;                                    // Menu en la que el jugador tiene su enfoque
        public static bool enMenu;
        public GameObject menuJugador;

        private void Start()
        {
            // Inicializa la variable de la instancia del sistema de menu
            instance = this.gameObject.GetComponent<MenuSystem>();
        }

        /* Metodo que cambia el menu activo por el enviado como parámetro y almacena en la pila
           el anterior menu activo 
           PRE: - menu -> GameObject valido
                - desactiva -> bool
           POST: menuFocus = menu
                 anterior_menuFocus apilado en MenuStack
                 desactiva = true -> anterior_menuFocus se desactiva */
        public static void SiguienteMenu(GameObject menu, bool desactiva)
        {
            if (menuStack.Count == 0 && !menuFocus)
            {
                menuFocus =  menu;
                enMenu = true;
            }
            else
            {
                menuStack.Push(menuFocus);
                if(desactiva)
                    menuFocus.SetActive(false);
                
                menuFocus = menu;  
            }
            
            menuFocus.SetActive(true);
        }
        
        /* Metodo que cambia el menu activo por el enviado como parámetro y almacena en la pila
           el anterior menu activo
           PRE: - menu -> GameObject valido
                - desactiva -> bool
           POST: menuFocus = menu
                 anterior_menuFocus apilado en MenuStack */
        public static void SiguienteMenu(GameObject menu)
        {
            if (menuStack.Count == 0 && !menuFocus)
            {
                menuFocus =  menu;
                enMenu = true;
            }
            else
            {
                menuStack.Push(menuFocus);
                menuFocus = menu;  
            }
            menu.SetActive(true);
        }

        /* Metodo que cambia el menu activo por el más recientemente apilado en menuStack y lo habilita
           PRE: menuStack:Count > 0 
           POST: deshabilita el actual menuFocus, desapila un elemento de menuStack, lo define como el actual
                 menuFocus y lo habilita
                 Devuelve el menu a activar*/
        public static GameObject MenuAnterior()
        {
            if (menuStack.Count > 0)
            {
                menuFocus.SetActive(false);
                menuFocus = menuStack.Pop();
                menuFocus.SetActive(true);
            }
            return menuFocus;
        }
        
        public static GameObject MenuAnterior(bool noDesactiva)
        {
            if (menuStack.Count > 0)
            {
                menuFocus = menuStack.Pop();
                menuFocus.SetActive(true);
            }
            return menuFocus;
        }

        /* Metodo que reinicia el sistema de menus, reiniciando la pila y nullificando el menuFocus
           POST: menuFocus != null -> desactiva menuFocus y menuFocus = null 
                 Se desactivan todos los elementos del menuStack y se limpia */
        public static void ResetMenuSystem()
        {
            if (menuFocus)
            {
                menuFocus.SetActive(false);
                menuFocus = null;
            }
            foreach (GameObject menu in menuStack)
            {
                menu.SetActive(false);
            }
        
            enMenu = false;
            menuStack.Clear();
        
        }
        
        /* Metodo que reinicia el sistema de menus, reiniciando la pila e inicializando el menuFocus con el
           menu pasado por parámetro.
           POST: anterior menuFocus != null -> desactiva anterior menuFocus
                 Se desactivan todos los elementos del menuStack y se limpia
                 Se establece el menuFocus con el menu pasado por parámetro */
        public static void ResetMenuSystem(GameObject menuInicio)
        {
            if(menuFocus) menuFocus.SetActive(false);
            foreach (GameObject menu in menuStack)
            {
                menu.SetActive(false);
            }
            menuStack.Clear();
            enMenu = true;
            menuFocus = menuInicio;
            menuFocus.SetActive(true);
        
        }

        public static void SetMenuFocus(GameObject menu)
        {
            menuFocus = menu;
        }
        
        public static GameObject GetMenuFocus()
        {
            return menuFocus;
        }
    }
}