using System;
using System.Collections;
using System.Collections.Generic;
using Codigo.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class Jugador : MonoBehaviour
{
    
    public float velocidad = 5f;
    public Vector3 movimiento;
    public DatosCombate.Estadisticas estadisticasBase; // Estadisticas base del jugador, nunca negativos ni 0 (no se modifican)
    public List<int> accionesJugador;                  // Lista de acciones que el jugador puede hacer si estuviera en combate
    public List<ObjectSlot> listaObjetos;
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
        
                                                                            
    }

    // Update is called once per frame
    void Update()
    {
        bool hablando = SistemaDialogo.instance != null && SistemaDialogo.instance.enDialogo;
        //string escena = SceneManager.GetActiveScene().name;
        // si no estamos ni en combate ni en dialogo ni en pausa
        if (!GLOBAL.enCombate && !hablando && !MenuPausa.enPausa && !MenuSystem.GetMenuFocus()) 
        {
            ControlMovimiento();
            sistemaInteraccion.DetectarInteraccion();
            
        } 
        else if (hablando && Input.GetKeyDown(KeyCode.F))
        {
            SistemaDialogo.instance.SiguienteFrase();
        }

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
        if (context.performed && !MenuSystem.GetMenuFocus())
        {
            //MenuSystem.instance.menuJugador.SetActive(true);
            MenuSystem.SiguienteMenu(MenuSystem.instance.menuJugador);
        }
    }
}
