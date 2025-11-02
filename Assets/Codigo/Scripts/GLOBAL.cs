using System.Collections.Generic;
using Codigo.Scripts;
using UnityEngine;

public class GLOBAL : MonoBehaviour
{
    public static GLOBAL instance;                      // Contiene la instacia del singleton
    public static List<Accion> acciones;                // Lista de todas las acciones de combate del juego
    public static List<DatosLuchador> combatientes;     // Lista de todos los datos de los luchadores (Sin uso o no se usa) 
    public static DatosGuardado guardado;               // Datos de guardado de la partida
    public static bool enCombate;                       // Indica si se esta dentro de combate o no
    public List<Accion> ListaAccionesTotales = new List<Accion>();
    public List<DatosLuchador> ListaDatosCombatiente = new List<DatosLuchador>();
    
    void Awake()
    {
        enCombate = false;
        instance = this;
        acciones = ListaAccionesTotales;
        combatientes = ListaDatosCombatiente;
        guardado = SistemaGuardado.Cargar();    // Carga los datos de guardado
    }
}
