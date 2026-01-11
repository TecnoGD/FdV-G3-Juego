using System;
using TMPro;
using UnityEngine.UI;

namespace Codigo.Scripts.Sistema_Menu
{
    public class MenuListaAcciones : Menu
    {
        public TMP_Text tipoAccionTexto;
        public TMP_Text potenciaAccionTexto;
        public TMP_Text descripcionAccionTexto;
        private void Awake()
        {
            defaultElementFocus.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            ActualizarLista();
        }

        public override void AccionPorDefecto()
        {
            tipoAccionTexto.gameObject.SetActive(true);
            potenciaAccionTexto.gameObject.SetActive(true);
            descripcionAccionTexto.gameObject.SetActive(true);
        }

        public override void SalidaPorDefecto()
        {
            tipoAccionTexto.gameObject.SetActive(false);
            potenciaAccionTexto.gameObject.SetActive(false);
            descripcionAccionTexto.gameObject.SetActive(false);
        }

        public void ActualizarLista()
        {
            var lista = GLOBAL.instance.Jugador.accionesJugador;
            var cantidadLista = lista.Count;
            var cantidadHijos = contenedoresDeSeleccionables[0].childCount;
            var i = 0;
            defaultElementFocus.gameObject.SetActive(true);
            for (; i<cantidadLista; i++)
            {
                if (i+1 < cantidadHijos)
                {
                    var hijo = gameObject.transform.GetChild(i+1).GetComponent<AccionMenuSlot>();
                    if (i == 0) lastElementFocus = hijo.gameObject.GetComponent<Selectable>();
                    hijo.indice = i;
                    hijo.nombreAccion.text = GLOBAL.acciones[lista[i]].Nombre;
                }
                else
                {
                    var copia = Instantiate(defaultElementFocus, gameObject.transform).GetComponent<AccionMenuSlot>();
                    if (i == 0) lastElementFocus = copia.gameObject.GetComponent<Selectable>();
                    copia.indice = i;
                    copia.nombreAccion.text = GLOBAL.acciones[lista[i]].Nombre;
                }
            }
            i++;
            for (; i < cantidadHijos; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
            
            defaultElementFocus.gameObject.SetActive(false);
        }
        
        public void ActualizarDatosAccion(int indice)
        {
            var accion =GLOBAL.acciones[GLOBAL.instance.Jugador.accionesJugador[indice]];
            if (accion as Ataque)
            {
                var ataque = accion as Ataque;
                var tipo = ataque.tipo switch
                {
                    Ataque.FISICO => "Tipo: FISICO",
                    Ataque.ESPECIAL => "Tipo: ESPECIAL",
                    _ => ""
                };
                tipoAccionTexto.text = tipo;
                potenciaAccionTexto.text = "Potencia: " + ataque.DañoBase;
            }
            else
            {
                tipoAccionTexto.text = "Tipo: SIN TIPO";
                potenciaAccionTexto.text = "Potencia: --";
            }
            descripcionAccionTexto.text = accion.Descripcion;
            
        }
        
    }
}