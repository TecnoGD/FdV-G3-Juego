using System.Collections;
using System.Collections.Generic;
using Codigo.Scripts;
using Codigo.Scripts.Sistema_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GLOBAL : MonoBehaviour
{
    public static GLOBAL instance;                      // Contiene la instacia del singleton
    public static List<Accion> acciones;                // Lista de todas las acciones de combate del juego
    public static List<DatosLuchador> combatientes;     // Lista de todos los datos de los luchadores (Sin uso o no se usa) 
    public static DatosGuardado guardado;               // Datos de guardado de la partida
    public static DatosConfig Configuracion;
    public static bool enCombate;                       // Indica si se esta dentro de combate o no
    public static bool EnEvento;      
    public List<Accion> ListaAccionesTotales = new List<Accion>();
    public List<DatosLuchador> ListaDatosCombatiente = new List<DatosLuchador>();
    public List<ObjetoConsumible> objetosConsumibles = new List<ObjetoConsumible>();
    public ObjetoConsumible objetivoPrueba;
    public Jugador Jugador;
    public List<Equipamiento> listaArmasTotal;
    public List<Equipamiento> listaArmaduraTotal;
    public List<Equipamiento> listaZapatosTotal;
    public List<Equipamiento> listaAccesoriosTotal;
    public List<Equipamiento>[] ListasDeEquipamientos;
    public List<int> listaObjetosConsumiblesTienda = new List<int>();
    public List<Equipamiento> listaEquipamientoTienda = new List<Equipamiento>();
    public List<CombateLayout> layoutsCombate = new List<CombateLayout>();
    public List<CombateLayout> layoutsBosses = new List<CombateLayout>();
    public static (int,int)[] limitesSpawn = new []{(0,7), (7,14), (14, 19)};
    public static List<int> batallasPlanificadas = new List<int>(new[] { 7, 16, 17, 24 });
    
    // Un diccionario para recordar: NPC -> Último charla leída"
    public Dictionary<string, int> memoriaNPCs = new Dictionary<string, int>();
    
    void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
        enCombate = false;
        instance = this;
        acciones = ListaAccionesTotales;
        combatientes = ListaDatosCombatiente;
        guardado = new DatosGuardado(objetivoPrueba);//SistemaGuardado.Cargar();    // Carga los datos de guardado
        Configuracion =  new DatosConfig();
        ListasDeEquipamientos = new [] {listaArmasTotal, listaArmaduraTotal,  listaZapatosTotal, listaAccesoriosTotal};
    }

    void Start()
    {
        
        //Object.DontDestroyOnLoad(MenuSystem.instance.menuJugador);
        
        Screen.SetResolution(Configuracion.width, Configuracion.height, Configuracion.fullScreen);
        //Object.DontDestroyOnLoad(NewMenuSystem.Instancia.menuJugador);
        //SceneManager.LoadScene("EscenaPrologo");
        
    }

    public void CambiarEscena(string escena, Vector3 posicion)
    {
        StartCoroutine(TestEspera(SceneManager.LoadSceneAsync(escena), posicion));
    }
    
    private IEnumerator TestEspera(AsyncOperation async, Vector3 posicion)
    {
        async.allowSceneActivation = false;
        var rig = GameObject.FindGameObjectWithTag("Rig Jugador").gameObject;
        rig.SetActive(false);
        //yield return null;
        yield return new WaitUntil(() => async.progress >= 0.9f);
        async.allowSceneActivation = true;
        yield return null;
        GameObject.FindGameObjectWithTag("Jugador").transform.position = posicion;
        yield return null;
        rig.SetActive(true);
        //yield return new WaitUntil(() => async.isDone);
        yield break;
    }

    // 1. Función para consultar si algo ya pasó
    public static bool TieneFlag(string idFlag)
    {
        // Si la lista está vacía o es nula, obviamente es falso
        if (guardado.flagsEventos == null) return false;

        return guardado.flagsEventos.Contains(idFlag);
    }

    // 2. Función para marcar que algo ha pasado
    public static void PonerFlag(string idFlag)
    {
        if (guardado.flagsEventos == null) guardado.flagsEventos = new List<string>();

        // Solo lo añadimos si no está ya, para no tener duplicados
        if (!guardado.flagsEventos.Contains(idFlag))
        {
            guardado.flagsEventos.Add(idFlag);
        }
    }

    public static void AumentarProgresoHistoria()
    {
        guardado.progresoHistoria++;
        switch (guardado.progresoHistoria)
        {
            case 8:
                guardado.actoActual = 2;
                break;
            case 18:
                guardado.actoActual = 3;
                break;
        }
    }
    
    void OnApplicationQuit()
    {
        //SistemaGuardado.GuardarConfiguracion(Configuracion);
    }
}
