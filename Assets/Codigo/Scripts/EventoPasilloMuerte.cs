using System.Collections;
using System.Collections.Generic; // Necesario para usar Listas
using UnityEngine;
using Codigo.Scripts; 

public class EventoPasilloMuerte : MonoBehaviour
{
    [Header("Configuración de Historia")]
    public int idEventoRequerido = 5; 

    [Header("Sistema de Luces (Tags)")]
    public string etiquetaLuces = "LuzAmbiente"; // Busca las luces con este Tag
    // public GameObject lucesDelPasillo; // <-- ESTO LO HEMOS QUITADO

    [Header("Efecto Ralentización")]
    public float velocidadLenta = 2.0f; 
    private float velocidadOriginal;

    [Header("Narrativa (El Eco)")]
    public string nombreVoz = "???"; 
    [TextArea] public string[] ecosDelPasado; 

    // Variables internas
    private bool eventoIniciado = false;
    private List<Light> lucesEncontradas = new List<Light>(); // Lista para guardar las luces

    private void Start()
    {
        // 1. BUSCAR LAS LUCES AUTOMÁTICAMENTE AL INICIO
        GameObject[] objetos = GameObject.FindGameObjectsWithTag(etiquetaLuces);
        foreach (GameObject obj in objetos)
        {
            Light l = obj.GetComponent<Light>();
            if (l != null) lucesEncontradas.Add(l);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Nota: Asegúrate de que tu personaje tiene el tag "Rig Jugador" o cámbialo aquí
        if (other.CompareTag("Rig Jugador") && !eventoIniciado)
        {
            if (GLOBAL.datosPartida.progresoHistoria == idEventoRequerido)
            {
                StartCoroutine(SecuenciaPesadilla());
            }
        }
    }

    private IEnumerator SecuenciaPesadilla()
    {
        eventoIniciado = true;
        GLOBAL.EnEvento = true;
        // 1. APAGÓN REPENTINO (Usando la lista de luces encontradas)
        foreach (Light l in lucesEncontradas)
        {
            if (l != null) l.enabled = false; // Apagamos el componente de luz
        }
        
        // 2. RALENTIZACIÓN
        if (GLOBAL.instance != null && GLOBAL.instance.Jugador != null)
        {
            velocidadOriginal = GLOBAL.instance.Jugador.velocidad; 
            GLOBAL.instance.Jugador.velocidad = velocidadLenta; 
        }

        yield return new WaitForSeconds(4.0f); 

        // 3. EL ECO (Diálogo)
        if (SistemaDialogo.instance != null)
        {
            SistemaDialogo.instance.IniciarDialogo(ecosDelPasado, nombreVoz, null);
            yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);
        }

        // 4. RECUPERACIÓN
        // Restauramos velocidad
        if (GLOBAL.instance != null && GLOBAL.instance.Jugador != null)
        {
            GLOBAL.instance.Jugador.velocidad = velocidadOriginal;
        }

        // Restauramos las luces (Las encendemos de nuevo)
        foreach (Light l in lucesEncontradas)
        {
            if (l != null) l.enabled = true;
        }

        // 5. GUARDAR PROGRESO
        GLOBAL.EnEvento = false;
        GLOBAL.AumentarProgresoHistoria();
    }
}
