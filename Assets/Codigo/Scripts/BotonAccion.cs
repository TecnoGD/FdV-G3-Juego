using Codigo.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class BotonAccion : MonoBehaviour
{
    
    public int accion = -1;     // atributo que indica la accion asignada a este boton
    void Start()
    {
        if (accion != -1)
        {
            gameObject.GetComponentInChildren<TMP_Text>().text = GLOBAL.acciones[accion].Nombre;    // Cambia el texto del boton por el nombre de la accion
            
            if (GLOBAL.acciones[accion].EstiloSeleccionObjetivo == Accion.MONOOBJETIVO ||
                GLOBAL.acciones[accion].EstiloSeleccionObjetivo == Accion.MULTIOBJETIVO)            // Dependiendo del tipo de objetivo de la accion, abrirá o no el menu de objetivos
                gameObject.GetComponent<MenuNavigator>().siguiente = SistemaCombate.instance.ElementosUI[SistemaCombate.UIObjetivoCombate];
            
        }
        else
        {
            gameObject.GetComponentInChildren<TMP_Text>().text = "Atras"; // Si el boton tiene asignada la accion nula
                                                                          // el boton es para retroceder al anterior menu
        }
    }

    public void OnClick()
    {
        ExecuteEvents.Execute<IMensajesCombate>(SistemaCombate.instance.gameObject, null,
            (x, y) => { x.AtaqueElegido(accion); }); // Ejecuta el evento de AtaqueElegido
                                                                                       // en el sistema de combate
    }
}
