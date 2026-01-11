using UnityEngine;
using UnityEngine.UI;

namespace Codigo.Scripts.Sistema_Menu
{
    public class ContextMenu : Menu
    {
        public override void AccionPorDefecto()
        {
            var obj = MenuObjetos.Instancia.objetoSeleccionado;
            if (obj)
            {
                var esCurativo = obj.objetoConsumible.objeto as ObjetoCurativo;
                seleccionables[0].gameObject.SetActive(esCurativo);
                if (!esCurativo)
                    seleccionables[1].GetComponent<Selectable>().Select();
                var pos = obj.transform.position;
                var posObj = MenuObjetos.Instancia.objetoSeleccionado.GetComponent<RectTransform>().rect;
                pos.x += posObj.width * 2;
                //pos.y -= posObj.height;
                gameObject.transform.position = pos;
            }
        }


        public void Equipar()
        {
            NewMenuSystem.SiguienteMenu(MenuObjetos.Instancia.menuObjetosEquipados);
        }
    }
}