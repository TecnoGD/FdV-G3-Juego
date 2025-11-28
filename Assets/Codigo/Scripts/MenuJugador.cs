using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UIElements.Toggle;

namespace Codigo.Scripts
{

    public class MenuJugador : Menu
    {
        private int _index;
        public GameObject currentTab;
        public GameObject tabs;
        public ToggleGroup tabsGroup;
        void Awake()
        {
            /*Object.DontDestroyOnLoad(gameObject);
            gameObject.SetActive(false);*/
        }

        public void SeleccionarTab(int tab)
        {
            if(currentTab)
                currentTab.SetActive(false);
            currentTab = tabs.transform.GetChild(tab).gameObject;
            currentTab.SetActive(true);
            _index = tab;
        }

        public void EntrarTab(Selectable seleccionable)
        {
            if (seleccionable && tabsGroup.AnyTogglesOn())
            {
                seleccionable.Select();
                MenuSystem.SiguienteMenu(currentTab);
                TabsControl(false);
            }
        }

        public void TabsControl(bool interactuable)
        {
            for (var i = 0; i < tabsGroup.transform.childCount; i++)
            {
                tabsGroup.transform.GetChild(i).gameObject.GetComponent<Selectable>().interactable = interactuable;
            }
        }

        void OnEnable()
        {
            //EventSystem.current.SetSelectedGameObject(primerFocus);
            primerFocus.SetActive(true);
            primerFocus.gameObject.GetComponent<Selectable>().Select();
            primerFocus.GetComponent<AutoChangeTab>().OnSelect(null);
            /*currentTab = tabs.transform.GetChild(0).gameObject;
            currentTab.SetActive(true);
            _index = 0;*/
        }

        public void Cancel(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (MenuSystem.GetMenuFocus() == gameObject)
                {
                    Debug.Log(gameObject.name);
                    MenuSystem.ResetMenuSystem();
                    currentTab.SetActive(false);
                    currentTab = null;
                    
                    primerFocus = tabsGroup.transform.GetChild(_index).gameObject;
                }
                else
                {
                    if (MenuSystem.GetMenuFocus().name == "ObjetosMenu" &&MenuSystem.MenuAnterior() == gameObject)
                    {
                        Debug.Log(MenuSystem.GetMenuFocus());
                        tabsGroup.SetAllTogglesOff();
                        TabsControl(true);
                        tabsGroup.transform.GetChild(_index).gameObject.GetComponent<Selectable>().Select();
                    }
                        
                }
            }
        }
    }
}