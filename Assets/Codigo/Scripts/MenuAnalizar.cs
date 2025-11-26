using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class MenuAnalizar  : MonoBehaviour
    {
        public GameObject panelAnalisis;        // Panel que muestra la información del enemigo
        public TMP_Text textoNombreAnalisis;    // Texto que muestra el nombre del enemigo
        public TMP_Text textoVidaAnalisis;      // Texto que muestra la vida del enemigo
        public TMP_Text textoAtaqueAnalisis;    // Texto que muestra el ataque del enemigo
        public TMP_Text textoDefensaAnalisis;   // Texto que muestra la defensa del enemigo
        public TMP_Text textoAtaqueEspecialAnalisis;   // Nuevo hueco para Ataque Especial
        public TMP_Text textoDefensaEspecialAnalisis;  // Nuevo hueco para Defensa Especial
        public UnityEngine.UI.Button botonVolverAnalisis;  // Botón para volver atrás desde el análisis
        
        /* Metodo que muestra la información del enemigo seleccionado en modo analizar
           PRE: indiceEnemigo -> int (índice del enemigo en la lista de luchadores)
           POST: Se muestra un panel con los atributos del enemigo seleccionado */
        public void MostrarAnalisisEnemigo(Luchador luchador)
        {
            // Rellenar los textos con la información del enemigo
            textoNombreAnalisis.text = luchador.nombre;
            textoVidaAnalisis.text = "Vida: " + luchador.vida;
            textoAtaqueAnalisis.text = "Ataque: " + luchador.estadisticas.ataque;
            textoDefensaAnalisis.text = "Defensa: " + luchador.estadisticas.defensa;
            textoAtaqueEspecialAnalisis.text = "Atq. Esp: " + luchador.estadisticas.ataqueEspecial;
            textoDefensaEspecialAnalisis.text = "Def. Esp: " + luchador.estadisticas.defensaEspecial;

            // 3. NUEVO: Forzar la selección del botón "Volver"
            // Primero limpiamos la selección actual para evitar "fantasmas"
            EventSystem.current.SetSelectedGameObject(null);
            // Ahora le decimos al sistema de eventos que seleccione nuestro botón
            EventSystem.current.SetSelectedGameObject(botonVolverAnalisis.gameObject);
        }
        
    }
}