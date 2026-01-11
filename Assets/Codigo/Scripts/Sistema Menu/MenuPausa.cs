using UnityEngine;
using UnityEngine.InputSystem;

namespace Codigo.Scripts.Sistema_Menu
{
    public class MenuPausa : Menu
    {
        public static MenuPausa instance; // instancia unica para que no haya duplicados
        public static bool enPausa = false; // variable para saber si el juego esta parado
        
        // el panel de la ui
        public GameObject menuPausaUI;
        public Menu menuAjustes;

        void Awake()
        {
            if (instance == null) // patron singleton: para asegurar que solo haya un sistema de pausa
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // si ya existe uno, borra el nuevo para no tener dos
                Destroy(gameObject);
            }
        }

        void OnDisable()
        {
            Time.timeScale = 1f;
            enPausa = false;
        }

        public override void SalidaPorDefecto()
        {
            if (NewMenuSystem.DentroDeUnMenu()) return;
            menuPausaUI.SetActive(false); 
            Time.timeScale = 1f;  // tiempo normal  
            enPausa = false;

        }

        public void Continuar()
        {
            menuPausaUI.SetActive(false); 
            Time.timeScale = 1f;  // tiempo normal
            NewMenuSystem.MenuAnterior();
            enPausa = false;
            
        }

        public void Pausar()
        {
            
            menuPausaUI.SetActive(true);
            Time.timeScale = 0f; // congelamos tiempo
            enPausa = true;
        }

        public void EntrarAjustes()
        {
            IrASubMenu(menuAjustes);
        }

        public void Salir()
        {
            Debug.Log("saliendo del juego...");
            Application.Quit();
        }
    }
}