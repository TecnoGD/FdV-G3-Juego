using Codigo.Scripts.Sistema_Menu;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Codigo.Scripts
{
    
    public class EquimientoEquipadoSlot : AutoChangeTab
    {
        public EquipamientoJugador index;
        public TMP_Text nombreTexto;
        private string _descripcionTexto;

        void OnEnable()
        {
            Refrescar();
        }

        public void Refrescar()
        {
            var equipamiento = GLOBAL.instance.Jugador.equipamientoJugador[(int)index];
            var tipoEquipamiento = (int)index;
            if (tipoEquipamiento > 3) tipoEquipamiento = 3;
            var listaApropiada = GLOBAL.instance.ListasDeEquipamientos[tipoEquipamiento];
            var listaApropiadaJugador = GLOBAL.instance.Jugador.ListasDeEquipamientosInventario[tipoEquipamiento];
            GLOBAL.instance.Jugador.ActualizarEstadisticas();
            
            if (equipamiento > -1 && equipamiento < listaApropiadaJugador.Count)
            {
                equipamiento = listaApropiadaJugador[equipamiento];
                nombreTexto.text = listaApropiada[equipamiento].nombre;
                _descripcionTexto =  listaApropiada[equipamiento].descripcion;
            }
            else
            {
                nombreTexto.text = "Vacio";
                _descripcionTexto = "";
            }
                
        }

        public override void OnSelect(BaseEventData eventData)
        {
            MenuSelectorEquipamiento.equipamientoAModificar = index;
            MenuSelectorEquipamiento.instance.descripcionSeleccion.text = _descripcionTexto;
            base.OnSelect(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            NewMenuSystem.SiguienteMenu(MenuSelectorEquipamiento.instance);
        }
    }
}