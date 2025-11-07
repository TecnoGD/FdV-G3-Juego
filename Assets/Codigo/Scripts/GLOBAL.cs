using System.Collections;
using System.Collections.Generic;
using Codigo.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GLOBAL : MonoBehaviour
{
    public static GLOBAL instance;                      // Contiene la instacia del singleton
    public static List<Accion> acciones;                // Lista de todas las acciones de combate del juego
    public static List<DatosLuchador> combatientes;     // Lista de todos los datos de los luchadores (Sin uso o no se usa) 
    public static DatosGuardado guardado;               // Datos de guardado de la partida
    public static bool enCombate;                       // Indica si se esta dentro de combate o no
    public List<Accion> ListaAccionesTotales = new List<Accion>();
    public List<DatosLuchador> ListaDatosCombatiente = new List<DatosLuchador>();
    public GameObject Jugador;
    
    void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
        enCombate = false;
        instance = this;
        acciones = ListaAccionesTotales;
        combatientes = ListaDatosCombatiente;
        guardado = SistemaGuardado.Cargar();    // Carga los datos de guardado
    }

    void Start()
    {
        SceneManager.LoadScene("SalaDescanso");
    }

    public void CambiarEscena(string escena, Vector3 posicion)
    {
        StartCoroutine(TestEspera(SceneManager.LoadSceneAsync(escena), posicion));
    }
    
    private IEnumerator TestEspera(AsyncOperation async, Vector3 posicion)
    {
        async.allowSceneActivation = false;
        //yield return null;
        yield return new WaitUntil(() => async.progress >= 0.9f);
        async.allowSceneActivation = true;
        yield return null;
        GameObject.FindGameObjectWithTag("Luchador Jugador").transform.position = posicion;
        //yield return new WaitUntil(() => async.isDone);
    }
}
