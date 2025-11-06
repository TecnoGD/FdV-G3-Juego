using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

/*
 * Contiene métodos que ayudan a configurar la navegación de la UI.
 * No se añade a ningún objeto en la escena.
 */
public static class MenuNavegacionE
{
    /*
     * Conecta una lista de elementos (botones, toggles)
     * en una navegación vertical cíclica (arriba/abajo)
     * pudiendo ser controlados por teclados Explícitamente
     */
    public static void ConfigurarNavegacionVertical(List<GameObject> elementos)
    {
        // si no hay elementos no hace nada
        if (elementos == null || elementos.Count == 0) return;

        // Recorre todos los elementos de la lista
        for (int i = 0; i < elementos.Count; i++)
        {
            // Usamos 'Selectable' porque funciona para Botones, Toggles, etc.
            Selectable elemento = elementos[i].GetComponent<Selectable>();
            if (elemento == null) continue; // Si no es navegable, saltar

            Navigation nav = new Navigation(); // Creamos un nuevo objeto de configuración de Navegación
            nav.mode = Navigation.Mode.Explicit; // Le decimos que usaremos navegación "Explícita" (manual)

            // Definir botón de ARRIBA (al seleccionar hacia arriba
            // Usamos un operador ternario para hacer un bucle:
            // Si (i == 0) (es el primer botón), su "arriba" es el ÚLTIMO (botones.Count - 1)
            // Si no, su "arriba" es el anterior (i - 1)
            int indiceArriba = (i == 0) ? elementos.Count - 1 : i - 1;
            nav.selectOnUp = elementos[indiceArriba].GetComponent<Selectable>();

            // Definir botón de ABAJO (al seleccionar hacia abajo
            // Lógica similar
            // Si (i == ultimo), su "abajo" es el PRIMERO (0)
            // Si no, es el siguiente (i + 1)
            int indiceAbajo = (i == elementos.Count - 1) ? 0 : i + 1;
            nav.selectOnDown = elementos[indiceAbajo].GetComponent<Selectable>();

            // Aplicar la navegación
            elemento.navigation = nav;
        }
    }

    public static void PonerFoco(List<GameObject> elementos)
    {
        if (elementos.Count > 0)
        {
            // "Soltamos" el foco del botón anterior (ej. el botón "Ataque" del menú principal)
            EventSystem.current.SetSelectedGameObject(null); 
            
            // "Agarramos" el foco y se lo damos al primer botón de nuestra lista (botones[0])
            EventSystem.current.SetSelectedGameObject(elementos[0]);
        }
        
    }
}