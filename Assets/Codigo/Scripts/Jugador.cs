using System;
using System.Collections;
using System.Collections.Generic;
using Codigo.Scripts;
using Codigo.Scripts.Sistema_Menu;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class Jugador : MonoBehaviour
{
    
    public float velocidad = 5f;
    public Vector3 movimiento;
    public DatosCombate.Estadisticas estadisticasBase;      // Estadisticas base del jugador, nunca negativos ni 0 (no se modifican)
    public DatosCombate.Estadisticas estadisticasEfectivas; // Estadisticas efectivas del jugador, nunca negativos ni 0 (no se modifican)
    public List<int> accionesJugador;                       // Lista de acciones que el jugador puede hacer si estuviera en combate
    public List<ObjectSlot> listaObjetos;
    public List<int>[] ListasDeEquipamientosInventario = new [] {new List<int>(), new List<int>(), new List<int>(), new List<int>()};
    public int[] equipamientoJugador = {-1,-1,-1,-1};
    public ObjectSlot[] objetosSeleccionadosCombate;
    public SistemaInteraccion sistemaInteraccion;
    
    
    

    private void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        sistemaInteraccion = gameObject.transform.GetChild(1).gameObject.GetComponent<SistemaInteraccion>();
        estadisticasBase = GLOBAL.guardado.estadisticasJugador;             // Carga las estadisticas base del jugador desde
                                                                            // el archivo de guardado
                                                                            
        accionesJugador = new List<int>(GLOBAL.guardado.accionesJugador);   // Carga las acciones del jugador desde
                                                                            // el archivo de guardado
        listaObjetos = new List<ObjectSlot>();
        foreach (var objeto in GLOBAL.guardado.objetosConsumibles)
        {
                
            var test = new ObjectSlot(GLOBAL.instance.objetosConsumibles[objeto.id], objeto.cantidad);
            listaObjetos.Add(test);
                
        }
        objetosSeleccionadosCombate = new ObjectSlot[4];
        for (int i = 0; i< GLOBAL.guardado.objetosSeleccionadosCombate.Length; i++)
        {
            var item = GLOBAL.guardado.objetosSeleccionadosCombate[i];
            if(item == -1)
                objetosSeleccionadosCombate[i] = new ObjectSlot(null, -1);
            else
                objetosSeleccionadosCombate[i] = listaObjetos[GLOBAL.guardado.objetosSeleccionadosCombate[i]];
        }
        GLOBAL.instance.Jugador.ActualizarEstadisticas();
        ListasDeEquipamientosInventario[0].Add(0);
        ListasDeEquipamientosInventario[1].Add(0);
        ListasDeEquipamientosInventario[2].Add(0);
        ListasDeEquipamientosInventario[3].Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        bool hablando = SistemaDialogo.instance != null && SistemaDialogo.instance.enDialogo;
        //string escena = SceneManager.GetActiveScene().name;
        // si no estamos ni en combate ni en dialogo ni en pausa
        if (!GLOBAL.enCombate && !hablando && !MenuPausa.enPausa && !NewMenuSystem.DentroDeUnMenu()) 
        {
            ControlMovimiento();
            sistemaInteraccion.DetectarInteraccion();
            
        } 
        else if (hablando && Input.GetKeyDown(KeyCode.F))
        {
            SistemaDialogo.instance.SiguienteFrase();
        }

    }

    public void ActualizarEstadisticas()
    {
        var modificadores = new int[5];
        var index = 0;
        for (var i = 0; i < 4; i++)
        {
            index = i;
            if (equipamientoJugador[i] < 0 || equipamientoJugador[i] >= ListasDeEquipamientosInventario[index].Count) continue;
            for(var j = 0; j < 5; j++)
                modificadores[j] += GLOBAL.instance.ListasDeEquipamientos[index][ListasDeEquipamientosInventario[index][equipamientoJugador[i]]].modificadorEstadisticas[j];
        }
        estadisticasEfectivas.vidaMax = estadisticasBase.vidaMax + modificadores[0];
        estadisticasEfectivas.ataque = estadisticasBase.ataque + modificadores[1];
        estadisticasEfectivas.defensa = estadisticasBase.defensa + modificadores[2];
        estadisticasEfectivas.ataqueEspecial = estadisticasBase.ataqueEspecial + modificadores[3];
        estadisticasEfectivas.defensaEspecial = estadisticasBase.defensaEspecial + modificadores[4];
    }
        
    private void ControlMovimiento()
    {
        movimiento = Vector3.zero;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            movimiento.x = -1;
        }    
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            movimiento.x = 1;
            
        transform.position += movimiento.normalized * (velocidad * Time.deltaTime); //calcular la pos
    }

    public void MenuJugador(InputAction.CallbackContext context)
    {
        if (context.performed && !NewMenuSystem.DentroDeUnMenu() && !MenuPausa.enPausa)
        {
            //MenuSystem.instance.menuJugador.SetActive(true);
            NewMenuSystem.SiguienteMenu(NewMenuSystem.Instancia.defaultMenus[0]);
        }
    }
}

public enum EquipamientoJugador
{
    Armas,
    Armadura,
    Zapatos,
    Accesorio1,
}
