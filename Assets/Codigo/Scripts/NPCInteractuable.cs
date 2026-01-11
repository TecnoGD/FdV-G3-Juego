using System.Collections.Generic;
using Codigo.Scripts.Sistema_Menu;
using UnityEngine;

namespace Codigo.Scripts
{
    public class NPCInteractuable : MonoBehaviour, IInteractuable
    {
        [System.Serializable] // para definir el struct de dialogo a Unity
        public struct EtapaDialogo
        {
            public string nota;     // nota interna para organizar los dialogos
            [TextArea(3, 10)]       // para un tamaño legible
            public string[] frases; // array que contiene las lineas de texto
        }
        public string nombreNPC = "Cubo";
        public Sprite fotoPerfil;                   // imagen que aparecera en la interfaz
        public GameObject iconoAlerta;              // objeto visual para avisar de nuevo dialogo
        public List<EtapaDialogo> etapasHistoria;   // lista con los diferentes dialogos segun la historia
        private int ultimaEtapaLeida = -1;          // variable privada para recordar si ya hablamos en este turno de historia
        public int offset = 1;
        public bool abreUnMenu;
        public Menu menuNpc;

        void Start()
        {
            // Preguntamos a GLOBAL si hemos hablado con este NPC antes
            if (GLOBAL.instance.memoriaNPCs.ContainsKey(nombreNPC))
            {
                // Si existe en la lista, recuperamos el dato
                ultimaEtapaLeida = GLOBAL.instance.memoriaNPCs[nombreNPC];
            }
            // al iniciar comprobamos si hay que mostrar la alerta
            ActualizarAlerta();
        }

        void Update()
        {
            // comprobamos constantemente por si ganas un combate y el progreso cambia
            ActualizarAlerta();
        }

        // metodo que se ejecuta al interactuar con el objeto
        public void Interactuar()
        {
            // obtenemos el progreso actual desde los datos globales
            int progresoActual = GLOBAL.guardado.progresoHistoria;
            
            // nos aseguramos de usar un indice valido dentro de la lista
            int indiceAUsar = Mathf.Clamp(progresoActual, 0, etapasHistoria.Count - 1);

            // llamamos al sistema de dialogo enviando las frases, el nombre y la foto
            SistemaDialogo.instance.IniciarDialogo(
                etapasHistoria[indiceAUsar].frases, 
                nombreNPC, 
                fotoPerfil,
                abreUnMenu,
                menuNpc
            );

            // registramos que ya hemos leido esta etapa para que no salga la alerta
            ultimaEtapaLeida = progresoActual;
            
            // indicar al GLOBAL que guarde que ya he hablado con este
            if (GLOBAL.instance.memoriaNPCs.ContainsKey(nombreNPC))
            {
                GLOBAL.instance.memoriaNPCs[nombreNPC] = ultimaEtapaLeida;
            }
            else
            {
                GLOBAL.instance.memoriaNPCs.Add(nombreNPC, ultimaEtapaLeida);
            }
            
            // actualizamos el estado del icono
            ActualizarAlerta();
        }

        // funcion para gestionar la visibilidad del icono de exclamacion
        private void ActualizarAlerta()
        {
            // si no hay icono asignado no hacemos nada
            if (iconoAlerta == null) return;

            int progresoGlobal = GLOBAL.guardado.progresoHistoria;

            // condiciones para mostrar el icono:
            // 1. el progreso actual es mayor a lo ultimo que leimos
            // 2. existe dialogo escrito para este momento de la historia
            bool esNuevo = progresoGlobal > ultimaEtapaLeida;
            bool existeDialogo = progresoGlobal < etapasHistoria.Count;

            if (esNuevo && existeDialogo)
            {
                iconoAlerta.SetActive(true); // encender el objeto visual
            }
            else
            {
                iconoAlerta.SetActive(false); // apagar el objeto visual
            }
        }
    }
}