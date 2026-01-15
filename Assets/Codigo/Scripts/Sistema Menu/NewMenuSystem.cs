using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements.InputSystem;

namespace Codigo.Scripts.Sistema_Menu
{
    public class NewMenuSystem : MonoBehaviour
    {
        public static NewMenuSystem Instancia;     //Referencia al Game Object que contiene el Menu System
        private static IMenu _currentMenu;        //Menu donde se tiene el foco actual
        public static Stack<IMenu> _pilaMenus;   //Pila de menus, menus anteriores al actual menu en foco
        public Menu[] defaultMenus;            //Contiene las referencias a los menus que son activados cuando no hay ninguno activo
        private static InputActionMap _accionesSinMenu;
        public static bool Desactivado = false;
        
        

        public static void Reinicializar(IMenu menu = null)
        {
            CerrarMenusYPila();
            EventSystem.current.SetSelectedGameObject(null);
            _pilaMenus = null;
            _pilaMenus =  new Stack<IMenu>();
            /*if(menu == null)
                //_accionesSinMenu.Disable();
            else
                //_accionesSinMenu.Enable();*/
            
            _currentMenu = menu;
            menu?.AbreMenu();
            
        }

        public static void SiguienteMenuInterno(IMenu menu, bool noDesactiva = true)
        {
            if (menu.EstaMenuBloqueado() || !SistemaDisponible()) return;
            
            if (_currentMenu != null)
            {
                _currentMenu.CierraMenu(noDesactiva);
                _pilaMenus.Push(_currentMenu);
            }
            
            //_accionesSinMenu.Disable();
            
            _currentMenu = menu;
            menu.AbreMenu();
        }
        
        public static void SiguienteMenu(IMenu menu)
        {
            if(menu != null) SiguienteMenuInterno(menu, ((Menu)menu).conservaAlCambiar);
        }

        public static IMenu MenuAnterior()
        {
            if(_currentMenu == null || !SistemaDisponible())
                return null;
            
            var menuADesactivar = _currentMenu;
            _currentMenu.CierraMenu();

            if (_pilaMenus.Count <= 0)
            {
                _currentMenu = null;
                //_accionesSinMenu.Enable();
                return menuADesactivar;
            }
                
            _currentMenu = _pilaMenus.Pop();
            _currentMenu.AbreMenu();
            
            return menuADesactivar;
        }

        public static bool DentroDeUnMenu()
        {
            return _currentMenu != null;
        }

        public static IMenu GetCurrentMenu()
        {
            return _currentMenu;
        }

        public static bool SoyMenuActual(IMenu menu)
        {
            return menu == _currentMenu;
        }
        
        private static void CerrarMenusYPila()
        {
            _currentMenu?.CierraMenuForzado();
            for (var i = 0; i < _pilaMenus.Count; i++)
            {
                _pilaMenus.Pop()?.CierraMenuForzado();
            }
        }
        
        private void Start()
        {
            Instancia = this;
            _currentMenu = null;
            _pilaMenus = new Stack<IMenu>();

            foreach (var menu in defaultMenus)
            {
                DontDestroyOnLoad(menu.gameObject);
            }
        }

        public void MenuJugador(InputAction.CallbackContext context)
        {   
            
            if (!DentroDeUnMenu() && context.performed)
            {
                SiguienteMenu(defaultMenus[0]);
            }
        }

        public static bool SistemaDisponible()
        {
            return !(SistemaDialogo.instance.enDialogo || GLOBAL.EnEvento || Desactivado);
        }
        
        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed && SistemaDisponible())
            {
                if (DentroDeUnMenu())
                {
                    if(!((Menu)_currentMenu).bloqueaVolver)
                        MenuAnterior();
                }
                else
                {
                    SiguienteMenu(defaultMenus[1]);
                    MenuPausa.instance.Pausar();
                }
            }
        }
    }
}