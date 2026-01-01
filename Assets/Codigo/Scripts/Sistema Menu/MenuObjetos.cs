using System;
using System.ComponentModel;
using UnityEngine;

namespace Codigo.Scripts.Sistema_Menu
{
    public class MenuObjetos : Menu
    {
        public static MenuObjetos Instancia;
        public ObjectSlotMenu objetoSeleccionado;
        public ContextMenu contextMenuObjetos;
        public Menu menuObjetosEquipados;


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

        void OnEnable()
        {
            if (contenedoresDeSeleccionables[0].childCount < GLOBAL.instance.Jugador.listaObjetos.Count){
                for (var i = contenedoresDeSeleccionables[0].childCount; i < GLOBAL.instance.Jugador.listaObjetos.Count; i++)
                {
                    Instantiate(defaultElementFocus, contenedoresDeSeleccionables[0]).GetComponent<ObjectSlotMenu>().index = i;
                    
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
        }
    }
}