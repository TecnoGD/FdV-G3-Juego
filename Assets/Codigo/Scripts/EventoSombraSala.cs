using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Necesario para listas
using Codigo.Scripts;

public class EventoSombraSala : MonoBehaviour
{
    [Header("CONFIGURACIÓN HISTORIA")]
    public int idEventoRequerido = 3; 
    public float esperaInicial = 1.5f; // Tiempo antes de que pase la sombra

    [Header("SISTEMA DE LUCES (TAGS)")]
    public string etiquetaLuces = "LuzAmbiente";
    
    // Configuración para cuando NO hay fantasmas (Luz cálida/blanca)
    public Color colorNormal = new Color(1f, 0.9f, 0.7f); 
    public float intensidadNormal = 1.0f;

    // Configuración para el SUSTO (Luz morada/oscura)
    public Color colorMiedo = new Color(0.5f, 0f, 0.8f); 
    public float intensidadMiedo = 0.5f;

    [Header("LA SOMBRA")]
    public GameObject objetoSombra;
    public Transform puntoInicial;
    public Transform puntoFinal;
    public float velocidadSombra = 6f;

    // Lista interna para guardar las luces que encontremos
    private List<Light> lucesEncontradas = new List<Light>();

    private void Start()
    {
        // 1. BUSCAR LAS LUCES POR ETIQUETA
        GameObject[] objetos = GameObject.FindGameObjectsWithTag(etiquetaLuces);
        foreach (GameObject obj in objetos)
        {
            Light l = obj.GetComponent<Light>();
            if (l != null) lucesEncontradas.Add(l);
        }

        int historiaActual = GLOBAL.guardado.progresoHistoria;

        // 2. DECIDIR QUÉ HACER SEGÚN LA HISTORIA
        
        // CASO A: Es el momento exacto (El evento ocurre ahora)
        if (historiaActual == idEventoRequerido)
        {
            // Empezamos con luz normal y lanzamos la secuencia
            AplicarColorLuces(colorNormal, intensidadNormal);
            StartCoroutine(SecuenciaEvento());
        }
        // CASO B: El evento YA pasó (Mantenemos la sala "maldita")
        else if (historiaActual > idEventoRequerido)
        {
            AplicarColorLuces(colorMiedo, intensidadMiedo);
            if(objetoSombra) objetoSombra.SetActive(false);
        }
        // CASO C: Aún no hemos llegado (Luz normal)
        else
        {
            AplicarColorLuces(colorNormal, intensidadNormal);
            if(objetoSombra) objetoSombra.SetActive(false);
        }
    }

    private IEnumerator SecuenciaEvento()
    {
        // 1. Esperamos un poco para que el jugador se sitúe
        yield return new WaitForSeconds(esperaInicial);

        // 2. ¡CAMBIO DE LUCES! (De normal a morado instantáneamente)
        AplicarColorLuces(colorMiedo, intensidadMiedo);

        // 3. MOVIMIENTO DE LA SOMBRA
        if (objetoSombra && puntoInicial && puntoFinal)
        {
            objetoSombra.SetActive(true);
            objetoSombra.transform.position = puntoInicial.position;

            while (Vector3.Distance(objetoSombra.transform.position, puntoFinal.position) > 0.1f)
            {
                objetoSombra.transform.position = Vector3.MoveTowards(objetoSombra.transform.position, puntoFinal.position, velocidadSombra * Time.deltaTime);
                yield return null;
            }
            objetoSombra.SetActive(false);
        }

        // 4. FINALIZAR
        yield return new WaitForSeconds(1.0f);
        // 5. FIN Y GUARDADO
        // El ambiente se queda morado y el juego continúa sin decir nada
        GLOBAL.AumentarProgresoHistoria();
    }

    // Función auxiliar para pintar todas las luces encontradas
    void AplicarColorLuces(Color col, float intens)
    {
        foreach (Light luz in lucesEncontradas)
        {
            luz.color = col;
            luz.intensity = intens;
        }
    }
}