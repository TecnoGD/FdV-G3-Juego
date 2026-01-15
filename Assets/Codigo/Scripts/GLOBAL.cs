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
    public static DatosGuardado datosPartida;               // Datos de guardado de la partida
    public static DatosGuardado DatosGuardado;               // Datos de guardado de la partida
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
    public static (int,int)[] limitesSpawn = new []{(0,10), (10,20), (20, 30)};
    public static List<int> batallasPlanificadas = new List<int>(new[] { 7, 16, 17, 24 });
    
    public AudioSource abrirMenuSonido;
    public AudioSource hoverMenuSonido;
    public AudioSource clickMenuSonido;
    
    // Un diccionario para recordar: NPC -> Último charla leída"
    public DiccionarioSerializableStringInt memoriaNPCs;
    
    void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
        enCombate = false;
        instance = this;
        acciones = ListaAccionesTotales;
        combatientes = ListaDatosCombatiente;
        datosPartida = new DatosGuardado(objetivoPrueba);//SistemaGuardado.Cargar();    // Carga los datos de guardado
        //SistemaGuardado.Cargar();
        DatosGuardado = (DatosGuardado)datosPartida.Clone();
        Configuracion =  new DatosConfig();
        ListasDeEquipamientos = new [] {listaArmasTotal, listaArmaduraTotal,  listaZapatosTotal, listaAccesoriosTotal};
        memoriaNPCs = datosPartida.memoriaNPCs;
    }

    void Start()
    {
        
        //Object.DontDestroyOnLoad(MenuSystem.instance.menuJugador);
        
        Screen.SetResolution(Configuracion.width, Configuracion.height, Configuracion.fullScreen);
        //Object.DontDestroyOnLoad(NewMenuSystem.Instancia.menuJugador);
        SceneManager.LoadScene("EscenaPrologo");
        
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
        if (datosPartida.flagsEventos == null) return false;

        return datosPartida.flagsEventos.Contains(idFlag);
    }

    // 2. Función para marcar que algo ha pasado
    public static void PonerFlag(string idFlag)
    {
        if (datosPartida.flagsEventos == null) datosPartida.flagsEventos = new List<string>();

        // Solo lo añadimos si no está ya, para no tener duplicados
        if (!datosPartida.flagsEventos.Contains(idFlag))
        {
            datosPartida.flagsEventos.Add(idFlag);
        }
    }

    public static void AumentarProgresoHistoria()
    {
        datosPartida.progresoHistoria++;
        switch (datosPartida.progresoHistoria)
        {
            case 8:
                datosPartida.actoActual = 2;
                break;
            case 18:
                datosPartida.actoActual = 3;
                break;
        }
    }

    public static void GuardarProgreso()
    {
        DatosGuardado = null;
        DatosGuardado = (DatosGuardado)datosPartida.Clone();
        DatosGuardado.vida = instance.Jugador.vida;
        DatosGuardado.estadisticasJugador = instance.Jugador.estadisticasBase;
        var lista = new List<DatosObjetoGuardado>();
        foreach (var objeto in instance.Jugador.listaObjetos)
        {
            lista.Add(new DatosObjetoGuardado(objeto.objeto.id, objeto.cantidad));
        }
        DatosGuardado.objetosConsumibles = null;
        DatosGuardado.objetosConsumibles = lista;
        
        DatosGuardado.objetosSeleccionadosCombate[0] =
            instance.Jugador.listaObjetos.IndexOf(instance.Jugador.objetosSeleccionadosCombate[0]);
        DatosGuardado.objetosSeleccionadosCombate[1] =
            instance.Jugador.listaObjetos.IndexOf(instance.Jugador.objetosSeleccionadosCombate[1]);
        DatosGuardado.objetosSeleccionadosCombate[2] =
            instance.Jugador.listaObjetos.IndexOf(instance.Jugador.objetosSeleccionadosCombate[2]);
        DatosGuardado.objetosSeleccionadosCombate[3] =
            instance.Jugador.listaObjetos.IndexOf(instance.Jugador.objetosSeleccionadosCombate[3]);
    }

    public static void CargarGuardado()
    {
        datosPartida = (DatosGuardado)DatosGuardado.Clone();
        instance.Jugador.CargarGuardado();
    }
    
    void OnApplicationQuit()
    {
        //SistemaGuardado.GuardarConfiguracion(Configuracion);
    }
}
