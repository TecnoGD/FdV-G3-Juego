using System;
using System.Collections.Generic;
using Codigo.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class Jugador : MonoBehaviour
{
    
    public float velocidad = 5f;
    public Vector3 movimiento;
    public DatosCombate.Estadisticas estadisticasBase; // Estadisticas base del jugador, nunca negativos ni 0 (no se modifican)
    public List<int> accionesJugador;                  // Lista de acciones que el jugador puede hacer si estuviera en combate


    private void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        estadisticasBase = GLOBAL.guardado.estadisticasJugador;             // Carga las estadisticas base del jugador desde
                                                                            // el archivo de guardado
                                                                            
        accionesJugador = new List<int>(GLOBAL.guardado.accionesJugador);   // Carga las acciones del jugador desde
                                                                            // el archivo de guardado
    }

    // Update is called once per frame
    void Update()
    {
        //string escena = SceneManager.GetActiveScene().name;
        if (!GLOBAL.enCombate)
            ControlMovimiento();
    }
        
    private void ControlMovimiento()
    {
        movimiento = Vector3.zero;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //mover hacia izq con A o <-
            movimiento.x = -1;
            
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //mover hacia der con D o ->
            movimiento.x = 1;
            
        transform.position += movimiento.normalized * (velocidad * Time.deltaTime); //calcular la pos
    }
}
