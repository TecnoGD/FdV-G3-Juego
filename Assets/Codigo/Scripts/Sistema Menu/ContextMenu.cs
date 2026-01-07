using UnityEngine;

namespace Codigo.Scripts.Sistema_Menu
{
    public class ContextMenu : Menu
    {
        public override void AccionPorDefecto()
        {
            var obj = MenuObjetos.Instancia.objetoSeleccionado;
            if (obj)
            {
                var pos = obj.transform.position;
                var posObj = MenuObjetos.Instancia.objetoSeleccionado.GetComponent<RectTransform>().rect;
                pos.x += posObj.width * 3;
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