using System;
using System.Collections.Generic;
using Codigo.Scripts;
using Codigo.Scripts.Sistema_Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;

public class MenuAtaques : Menu
{
    public GameObject prefabButton;     // Prefab del boton del ataque 
    public GameObject prefabBotonAtras; // Prefab del boton para volver al menu anterior
    
    
    private List<GameObject> botones = new List<GameObject>(); // almacena los botones creados para poder navegar entre ellos
    
    /* Metodo llamado como mensaje por el Sistema de combate, el cual inicializa los botones para que el jugador 
       pueda seleccionar el ataque */
    public void InicioCombate()
    {
        //limpiamos la lista por si esto se llamara una segunda vez, que no debería
        botones.Clear();
        // Obtiene la lista de acciones del jugador
        List<int> acciones = SistemaCombate.instance.jugador.listaAcciones; 
        foreach (int accion in acciones)
        {
            // Crea el botón en la escena
            GameObject newButton = Instantiate(prefabButton, contenedoresDeSeleccionables[0]);
            
            // Obtenemos el script 'BotonAccion' que tiene ese prefab
            BotonAccion newButtonScript = newButton.GetComponent<BotonAccion>();
            
            // Le asignamos el ID del ataque para que sepa qué hacer al pulsarlo
            newButtonScript.accion = accion; 
            
            // Guardamos el botón en nuestra lista
            botones.Add(newButton);
        }
        // Crea e instancia el boton del volver al anterior menu
        //GameObject botonAtras = Instantiate(prefabBotonAtras, contenedoresDeSeleccionables[0]);
        
        // Añade también el botón "Atrás" a la lista de navegación
        //botones.Add(botonAtras);
        defaultElementFocus = botones[0].GetComponent<Selectable>();
        // Llamamos a nuestro script que configura la navegación Automática
        foreach (GameObject button in botones)
        {
            var navigation = button.gameObject.GetComponent<Button>().navigation;
            navigation.mode = Navigation.Mode.Automatic;
            button.gameObject.GetComponent<Button>().navigation = navigation;
        }
        //MenuNavegacionE.ConfigurarNavegacionVertical(botones);
    }

    /*
     * MÉTODO DE FOCO (llamando Script MenuNavegacionE
     * Se llama automáticamente CADA VEZ que este GameObject (el panel) se activa.
     */

    public override void AccionPorDefecto()
    {
        SistemaCombate.instance.panelInfoAcciones.SetActive(true);
    }

    public override void SalidaPorDefecto()
    {
        SistemaCombate.instance.panelInfoAcciones.SetActive(false);
    }
    
    

    private void OnEnable()
    {
        MenuNavegacionE.PonerFoco(botones);
    }
}
