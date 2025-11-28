using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Codigo.Scripts
{
    public class MenuPausa : MonoBehaviour
    {
        public static MenuPausa instance; // instancia unica para que no haya duplicados
        public static bool enPausa = false; // variable para saber si el juego esta parado
        
        // el panel de la ui
        public GameObject menuPausaUI;

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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // Esc
            {
                if (enPausa)
                {
                    Continuar();
                }
                else
                {
                    Pausar();
                }
            }
        }

        public void Continuar()
        {
            menuPausaUI.SetActive(false); 
            Time.timeScale = 1f;  // tiempo normal  
            enPausa = false;              
        }

        void Pausar()
        {
            menuPausaUI.SetActive(true);  
            Time.timeScale = 0f;     // congelamos tiempo
            enPausa = true;   
            
        }

        public void Salir()
        {
            Debug.Log("saliendo del juego...");
            Application.Quit();
        }
    }
}