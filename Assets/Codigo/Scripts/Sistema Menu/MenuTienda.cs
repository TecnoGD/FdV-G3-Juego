using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Codigo.Scripts.Sistema_Menu
{
    public class MenuTienda : Menu
    {
        public static MenuTienda instance;
        public TiendaSlot templateConsumible;
        public TiendaSlot templateEquipamiento;
        public TMP_Text descripcionTexto;
        public TMP_Text dineroTexto;
        public Sprite[] TexturaTipo;

        private void Awake()
        {
            instance = this;
        }

        void OnEnable()
        {
            var camara = Camera.main.GetComponent<CamaraSeguimiento>();
            var objetivoActual = camara.ObtenerObjetivo().position;
            objetivoActual.x += 5;
            camara.EnfocarPuntoPosicion(objetivoActual);
            ActualizarDinero();
        }

        void ActualizarDinero()
        {
            dineroTexto.text = "Dinero:" + GLOBAL.instance.Jugador.dinero + "$";
        }

        public void ActualizarOpcionesConsumibles()
        {
            descripcionTexto.text = "";
            for (var i = 2; i < contenedoresDeSeleccionables[0].childCount; i++)
            {
                Destroy(contenedoresDeSeleccionables[0].GetChild(i).gameObject);
            }
            
            templateConsumible.gameObject.SetActive(true);
            for (var i = 0; i < GLOBAL.instance.listaObjetosConsumiblesTienda.Count; i++)
            {
                var slot = Instantiate(templateConsumible, contenedoresDeSeleccionables[0]).GetComponent<TiendaSlot>();
                slot.idObjeto = i;
                slot.tipoObjeto = 0;
                var objeto = GLOBAL.instance
                    .objetosConsumibles[GLOBAL.instance.listaObjetosConsumiblesTienda[i]];
                slot.nombreObjeto.text = objeto.nombre;
                slot.descripcionObjeto = objeto.descripcion;
                slot.precio = objeto.precio;
                slot.precioObjeto.text = objeto.precio + "$";
                slot.texturaObjeto.sprite = objeto.textura;
                if(i == 0) defaultElementFocus = slot.gameObject.GetComponent<Selectable>();
            }
            templateConsumible.gameObject.SetActive(false);
            
        }
        
        public void ActualizarOpcionesEquipamientos()
        {
            descripcionTexto.text = "";
            for (var i = 2; i < contenedoresDeSeleccionables[0].childCount; i++)
            {
                Destroy(contenedoresDeSeleccionables[0].GetChild(i).gameObject);
            }
            
            templateEquipamiento.gameObject.SetActive(true);
            for (var i = 0; i < GLOBAL.instance.listaEquipamientoTienda.Count; i++)
            {
                var slot = Instantiate(templateEquipamiento, contenedoresDeSeleccionables[0]).GetComponent<TiendaSlot>();
                var objeto = GLOBAL.instance.listaEquipamientoTienda[i];
                var indice = GLOBAL.instance.ListasDeEquipamientos[(int)objeto.tipoEquipamiento].IndexOf(objeto);
                
                slot.idObjeto = indice;
                slot.tipoObjeto = 1;
                slot.nombreObjeto.text = objeto.nombre;
                slot.descripcionObjeto = objeto.descripcion;

                if (GLOBAL.instance.Jugador.ListasDeEquipamientosInventario[(int)objeto.tipoEquipamiento]
                    .Contains(indice))
                {
                    slot.precio = -1;
                    slot.precioObjeto.text = "Agotado";
                }
                else
                {
                    slot.precio = objeto.precio;
                    slot.precioObjeto.text = objeto.precio + "$";
                }
                
                slot.equipamientoTipo = objeto.tipoEquipamiento;
                slot.texturaObjeto.sprite = TexturaTipo[(int)objeto.tipoEquipamiento];
                if(i == 0) defaultElementFocus = slot.gameObject.GetComponent<Selectable>();
            }
            templateEquipamiento.gameObject.SetActive(false);
            
        }

        public static void EfectuarCompra(int idObjeto, int tipoObjeto, TipoEquipamiento equipamientoTipo, int precio)
        {

            if (GLOBAL.instance.Jugador.dinero < precio)
            {
                MenuTienda.instance.descripcionTexto.text = "Lo siento, pero no tienes suficiente dinero";
                return;
            }
            
            if (precio == -1)
            {
                MenuTienda.instance.descripcionTexto.text = "Lo siento, pero ese producto esta agotado";
                return;
            }
                
            if (tipoObjeto == 0)
            {
                var loTienes = false;
                var objeto = GLOBAL.instance.objetosConsumibles[GLOBAL.instance.listaObjetosConsumiblesTienda[idObjeto]];
                foreach (var objectSlot in GLOBAL.instance.Jugador.listaObjetos.Where(objectSlot =>
                             Equals(objectSlot.objeto, objeto)))
                {
                    objectSlot.cantidad++;
                    loTienes = true;
                    break;
                }

                if (!loTienes) GLOBAL.instance.Jugador.listaObjetos.Add(new ObjectSlot(objeto, 1));
            }
            else
            {
                Debug.Log("test");
                GLOBAL.instance.Jugador.ListasDeEquipamientosInventario[(int)equipamientoTipo].Add(idObjeto);
                
            }
            MenuTienda.instance.descripcionTexto.text = "¡Gracias por su compra!";
            GLOBAL.instance.Jugador.dinero -= precio;
            MenuTienda.instance.ActualizarDinero();
            
        }

        public void CambiarDescripcion(string descripcion)
        {
            descripcionTexto.text = descripcion;
        }
    }
}