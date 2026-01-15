using System.Collections.Generic;
using Codigo.Scripts.Sistema_Menu;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class SistemaDialogo : MonoBehaviour
    {
        // instancia estatica para acceder desde cualquier script facilmente
        public static SistemaDialogo instance;
        
        // referencias a los elementos de la ui en el canvas
        public GameObject panelDialogo; 
        public TMP_Text textoDialogo; 
        
        // elementos para mostrar quien esta hablando
        public TMP_Text textoNombreNPC;             // componente de texto para el nombre
        public Image imagenPerfilNPC;               // componente de imagen para la foto
        private Sprite spritePorDefecto;            // variable para guardar la imagen inicial y usarla si no nos pasan otra
        
        public bool usarInputInterno = false;
                                    
        public bool enDialogo = false;              // variable para saber si estamos hablando y bloquear movimiento
        private Queue<string> colaFrases;           // cola para guardar las frases y sacarlas una a una en orden
        
        public static bool AbreUnMenuAlTerminar = false;
        public static Menu menuFinDialogo;
        

        void Awake()
        {
            Object.DontDestroyOnLoad(gameObject);
            
            // configuracion del singleton
            if (instance == null) instance = this;
            
            // inicializamos la cola vacia
            colaFrases = new Queue<string>();
            
            // nos aseguramos de que el panel este cerrado al arrancar el juego
            panelDialogo.SetActive(false); 
            
            // guardamos la imagen que pusiste en el editor como la imagen por defecto
            if (imagenPerfilNPC != null)
            {
                spritePorDefecto = imagenPerfilNPC.sprite;
            }
        }

        void Update()
        {
            // Si estamos en dialogo y pulsamos F
            
        }

        public void SiguienteDialogo(InputAction.CallbackContext context)
        {
            if (context.performed && enDialogo)
            {
                SiguienteFrase();
            }
        }

        // metodo que llaman los npcs para empezar a hablar
        public void IniciarDialogo(string[] frases, string nombre, Sprite imagen, bool abreMenu = false, Menu menu = null)
        {
            AbreUnMenuAlTerminar =  abreMenu;
            menuFinDialogo = menu;
            
            // activamos el estado de dialogo para detener al jugador
            enDialogo = true;
            
            // mostramos la ventana de dialogo
            panelDialogo.SetActive(true);
            
            // ponemos el nombre del personaje en la ui
            textoNombreNPC.text = nombre;

            // logica para decidir que foto mostrar
            if (imagen != null) 
            {
                // si el npc tiene foto especifica, usamos esa
                imagenPerfilNPC.sprite = imagen;
            }
            else 
            {
                // si no tiene foto (es null), ponemos la imagen por defecto guardada al inicio
                imagenPerfilNPC.sprite = spritePorDefecto;
            }
            
            // nos aseguramos de que el objeto imagen este visible
            imagenPerfilNPC.gameObject.SetActive(true);

            // limpiamos frases viejas y metemos las nuevas en la cola
            colaFrases.Clear();
            foreach (string frase in frases)
            {
                colaFrases.Enqueue(frase);
            }

            // mostramos la primera frase inmediatamente
            SiguienteFrase();
        }

        // metodo para pasar al siguiente texto cuando pulsamos f
        public void SiguienteFrase()
        {
            // si ya no quedan frases en la cola, cerramos el dialogo
            GLOBAL.instance.clickMenuSonido.PlayOneShot(GLOBAL.instance.clickMenuSonido.clip);
            if (colaFrases.Count == 0)
            {
                TerminarDialogo();
                if(AbreUnMenuAlTerminar)
                    NewMenuSystem.SiguienteMenu(menuFinDialogo);
                return;
            }

            // sacamos la siguiente frase de la cola y la ponemos en pantalla
            string frase = colaFrases.Dequeue();
            textoDialogo.text = frase;
        }

        // metodo interno para cerrar al acabar
        void TerminarDialogo()
        {
            // liberamos al jugador para que se mueva
            enDialogo = false;
            
            // ocultamos el panel visualmente
            panelDialogo.SetActive(false);
        }
    }
}