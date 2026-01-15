using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Codigo.Scripts.Sistema_Menu
{
    public class Menu : MonoBehaviour, IMenu
    {
        public bool seMuestraEnAnteriorMenu; //Si el Menu es visible en el menu anterior aun cuando no es el foco actual
        public bool bloqueoMenu;
        public bool conservaAlCambiar;
        public bool recordarUltimoSeleccionado;
        public bool bloqueaVolver;
        public bool bloquearAutoSeleccionado;
        public bool reproduceSonidoMenu;
        public Selectable defaultElementFocus;
        public Selectable lastElementFocus;
        public GameObject[] elementosGraficos; 
        public Selectable[] seleccionables; //Para seleccionables estáticos
        public Transform[] contenedoresDeSubMenus;
        public Transform[] contenedoresDeSeleccionables; //Para seleccionables dinámicos o muy numerosos
        

        

        public virtual void AbreMenu()
        {
            PrecargaMenu();
            CambiarEstadoSeleccionables(true);
            if (reproduceSonidoMenu && !gameObject.activeSelf)
                GLOBAL.instance.abrirMenuSonido.PlayOneShot(GLOBAL.instance.abrirMenuSonido.clip);
            
            MostrarMenu();
            if (!bloquearAutoSeleccionado)
            {
                EventSystem.current.SetSelectedGameObject(null);

                if (recordarUltimoSeleccionado && lastElementFocus)
                    lastElementFocus.Select();
                else
                    defaultElementFocus?.Select();
            }

            AccionPorDefecto();
        }

        public void MostrarMenu()
        {
            if (gameObject.activeInHierarchy) return;
            gameObject.SetActive(true);
            //MostrarSubMenus();
        }

        /*public void MostrarComoSubMenu()
        {
            if(seMuestraEnAnteriorMenu) MostrarMenu();
        }

        public void MostrarSubMenus()
        {
            foreach (var subMenu in subMenus)
            {
                subMenu.MostrarComoSubMenu();
            }
        }*/

        public virtual void CierraMenu(bool noDesactivar = true)
        {
            if(!bloquearAutoSeleccionado && recordarUltimoSeleccionado) lastElementFocus = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            CambiarEstadoSeleccionables(false);
            SalidaPorDefecto();
            if(!seMuestraEnAnteriorMenu)
                gameObject.SetActive(noDesactivar);
            
        }
        
        public void CierraMenuForzado()
        {
            CambiarEstadoSeleccionables(false);
            gameObject.SetActive(false);
        }

        public void IrASubMenu(Menu subMenu)
        {
            NewMenuSystem.SiguienteMenuInterno(subMenu, conservaAlCambiar);
        }

        public virtual void VolverAMenuAnterior()
        {
            NewMenuSystem.MenuAnterior();
        }

        public void CambiarEstadoSeleccionables(bool estado)
        {
            foreach (var seleccionable in seleccionables)
            {
                seleccionable.interactable = estado;
            }

            foreach (var contenedor in contenedoresDeSeleccionables)
            {
                for (var i = 0; i < contenedor.childCount; i++)
                {
                    var child = contenedor.GetChild(i);
                    if (child)
                        child.gameObject.GetComponent<Selectable>().interactable = estado;
                }
            }
        }

        public virtual void PrecargaMenu()
        {
            return;
        }

        public virtual void AccionPorDefecto()
        {
            return;
        }
        
        public virtual void SalidaPorDefecto()
        {
            return;
        }

        public virtual bool EstaMenuBloqueado()
        {
            return bloqueoMenu;
        }
    }
}