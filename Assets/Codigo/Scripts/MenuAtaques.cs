using System;
using System.Collections.Generic;
using Codigo.Scripts;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class MenuAtaques : MonoBehaviour
{
    public GameObject prefabButton;     // Prefab del boton del ataque 
    public GameObject prefabBotonAtras; // Prefab del boton para volver al menu anterior
    
    /* Metodo llamado como mensaje por el Sistema de combate, el cual inicializa los botones para que el jugador 
       pueda seleccionar el ataque */
    public void InicioCombate()
    {
        // Obtiene la lista de acciones del jugador
        List<int> acciones = SistemaCombate.instance.jugador.listaAcciones; 
        foreach (int accion in acciones)
        {
            // Crea e instancia el boton de la accion 
            BotonAccion newButton = Instantiate(prefabButton, gameObject.transform).GetComponent<BotonAccion>();
            newButton.accion = accion; // Asigna al boton creado la accion que le corresponde
        }
        Instantiate(prefabBotonAtras, gameObject.transform); // Crea e instancia el boton del volver al anterior menu
    }
}
