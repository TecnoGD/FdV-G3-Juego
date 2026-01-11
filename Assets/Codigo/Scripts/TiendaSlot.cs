using Codigo.Scripts.Sistema_Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class TiendaSlot : ScrollUpdater
    {
        public int idObjeto;
        public int tipoObjeto; //0 consumible, 1 equipamiento
        public TipoEquipamiento equipamientoTipo;
        public Image texturaObjeto;
        public TMP_Text nombreObjeto;
        public string descripcionObjeto;
        public int precio;
        public TMP_Text precioObjeto;
        public UnityEvent<string> cambiarDescripcion;

        public void RealizarCompra()
        {
            MenuTienda.EfectuarCompra(idObjeto, tipoObjeto, equipamientoTipo, precio);
            if (tipoObjeto == 1)
            {
                precio = -1;
                precioObjeto.text = "Agotado";
            }
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            cambiarDescripcion.Invoke(descripcionObjeto);
        }
    }
    
}