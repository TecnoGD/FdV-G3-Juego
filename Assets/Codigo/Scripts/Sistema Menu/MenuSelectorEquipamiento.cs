using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Codigo.Scripts.Sistema_Menu
{
    public class MenuSelectorEquipamiento : Menu
    {
        public static MenuSelectorEquipamiento instance;
        public static EquipamientoJugador equipamientoAModificar;
        public TMP_Text descripcionSeleccion;
        public Sprite[] TexturaTipo;
        public TMP_Text[] comparadoresEstadisticas;
        public UnityEvent refrescoEquipamiento;

        public void Start()
        {
            instance = this;
        }

        public void ActualizarLista(int tipoEquipamiento)
        {
            var lista = GLOBAL.instance.Jugador.ListasDeEquipamientosInventario[tipoEquipamiento];
            var cantidadLista = lista.Count;
            var cantidadHijos = contenedoresDeSeleccionables[0].childCount;
            var i = 0;
            for (; i<cantidadLista; i++)
            {
                if (i+1 < cantidadHijos)
                {
                    var hijo = gameObject.transform.GetChild(i+1).GetComponent<EquipamientoSlot>();
                    hijo.index = i;
                    hijo.lista = tipoEquipamiento;
                    hijo.texturaTipoEquipamiento.sprite = TexturaTipo[tipoEquipamiento];
                }
                else
                {
                    var copia = Instantiate(defaultElementFocus, gameObject.transform).GetComponent<EquipamientoSlot>();
                    copia.index = i;
                    copia.lista = tipoEquipamiento;
                    copia.texturaTipoEquipamiento.sprite = TexturaTipo[tipoEquipamiento];
                }
            }
            i++;
            for (; i < cantidadHijos; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
            gameObject.BroadcastMessage("Refresco");
        }

        public void RefrescoEquipamiento()
        {
            gameObject.BroadcastMessage("Refresco");
        }

        

        public override void SalidaPorDefecto()
        {
            for (var i = 0; i < 5; i++)
                comparadoresEstadisticas[i].gameObject.SetActive(false);
        }

        public void ActualizarComparadores(int[] modificadoresAntiguos, int[] modificadoresNuevos)
        {
            for (var i = 0; i < 5; i++)
            {
                modificadoresNuevos[i] -= modificadoresAntiguos[i];
                if (modificadoresNuevos[i] == 0)
                {
                    comparadoresEstadisticas[i].gameObject.SetActive(false);
                    continue;
                }
                comparadoresEstadisticas[i].gameObject.SetActive(true);
                if (modificadoresNuevos[i] > 0)
                {
                    comparadoresEstadisticas[i].color = Color.limeGreen;
                    comparadoresEstadisticas[i].text = "(+" + modificadoresNuevos[i] + ")";
                }
                else
                {
                    comparadoresEstadisticas[i].color = Color.softRed;
                    comparadoresEstadisticas[i].text = "(" + modificadoresNuevos[i] + ")";
                }
                    
            }
        }
    }
}