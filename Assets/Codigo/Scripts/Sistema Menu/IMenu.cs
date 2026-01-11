using UnityEngine.EventSystems;

namespace Codigo.Scripts.Sistema_Menu
{
    public interface IMenu
    {
        
        public void AbreMenu();
        public void MostrarMenu();
        //public void MostrarComoSubMenu();
        //public void MostrarSubMenus();
        public void CierraMenu(bool noDesactivar = false);
        public void CierraMenuForzado();
        public void IrASubMenu(Menu indice);
        public void VolverAMenuAnterior();
        public void CambiarEstadoSeleccionables(bool estado);
        public void PrecargaMenu();
        public void AccionPorDefecto();
        public void SalidaPorDefecto();
        public bool EstaMenuBloqueado();


    }
}