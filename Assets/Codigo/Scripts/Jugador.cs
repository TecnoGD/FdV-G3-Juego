using System.Collections.Generic;
using Codigo.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jugador : MonoBehaviour
{
    public class Jugador : Luchador
    {
        public float velocidad = 5f;
        public Vector3 movimiento;
        public DatosCombate.Estadisticas estadisticasBase; // Estadisticas base del jugador, nunca negativos ni 0 (no se modifican)
        public List<int> accionesJugador;                  // Lista de acciones que el jugador puede hacer si estuviera en combate
        
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
            string escena = SceneManager.GetActiveScene().name;
            if (escena == "SalaDescanso")
                ControlMovimiento();
        }
        
        private void ControlMovimiento()
        {
            movimiento = Vector3.zero;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                movimiento.x = -1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                movimiento.x = 1;
            
            Mover(movimiento);
        }
        
        private void Mover(Vector3 movimiento)
        {
            transform.position += movimiento.normalized * (velocidad * Time.deltaTime);
        }    
}
