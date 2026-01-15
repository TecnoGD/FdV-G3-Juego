using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Codigo.Scripts.Sistema_Menu
{
    public class MenuObjetos : Menu
    {
        public static MenuObjetos Instancia;
        public ObjectSlotMenu objetoSeleccionado;
        public GameObject prefabSlot;
        public ContextMenu contextMenuObjetos;
        public Menu menuObjetosEquipados;
        public MostrarDatosObjeto mostradorDatos;


        private void Start()
        {
            Instancia = this;
        }

        public void ObjetoSeleccionado(ObjectSlotMenu objeto)
        {
            objetoSeleccionado = objeto;
        }

        public override bool EstaMenuBloqueado()
        {
            var resultado = false;
            if (!bloqueoMenu)
                resultado = contenedoresDeSeleccionables[0].childCount <= 0;
            
            return resultado;
        }

        private void OnEnable()
        {
            AccionPorDefecto();
        }


        public override void AccionPorDefecto()
        {
            if (contenedoresDeSeleccionables[0].childCount < GLOBAL.instance.Jugador.listaObjetos.Count){
                for (var i = contenedoresDeSeleccionables[0].childCount; i < GLOBAL.instance.Jugador.listaObjetos.Count; i++)
                {
                    Instantiate(prefabSlot, contenedoresDeSeleccionables[0]).GetComponent<ObjectSlotMenu>().index = i;
                    
                }
            }else if (contenedoresDeSeleccionables[0].childCount > GLOBAL.instance.Jugador.listaObjetos.Count)
            {
                for (var i = GLOBAL.instance.Jugador.listaObjetos.Count; i < contenedoresDeSeleccionables[0].childCount; i++)
                {
                    Destroy(contenedoresDeSeleccionables[0].GetChild(i).gameObject);
                }
            }
            if(GLOBAL.instance.Jugador.listaObjetos.Count > 0)
                contenedoresDeSeleccionables[0].BroadcastMessage("Refresco");

            StartCoroutine(DespuesFrame());


        }

        IEnumerator DespuesFrame()
        {
            yield return null;
            if (contenedoresDeSeleccionables[0].childCount > 0)
            {
                defaultElementFocus = contenedoresDeSeleccionables[0].GetChild(0).GetComponent<Selectable>();
                yield break;
            }
            if(NewMenuSystem.SoyMenuActual(this))
                NewMenuSystem.MenuAnterior();
        }
    }
}