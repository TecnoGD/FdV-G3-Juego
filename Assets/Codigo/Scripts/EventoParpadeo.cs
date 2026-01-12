using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Necesario para usar Listas
using Codigo.Scripts;

public class EventoParpadeo : MonoBehaviour
{
    [Header("Configuración")]
    [Range(0, 100)] public float probabilidad = 100f; 
    public string etiquetaLuces = "LuzAmbiente"; // La etiqueta a buscar

    [Header("Audio")]
    public AudioSource audioChisporroteo; 

    // Variables internas
    private bool lucesRotas = false; 
    private List<Light> lucesEncontradas = new List<Light>(); // Lista dinámica

    private void Start()
    {
        // 1. BUSCAR LAS LUCES AUTOMÁTICAMENTE
        // Lo hacemos antes de nada para tener la lista lista
        GameObject[] objetos = GameObject.FindGameObjectsWithTag(etiquetaLuces);
        foreach (GameObject obj in objetos)
        {
            Light l = obj.GetComponent<Light>();
            if (l != null) lucesEncontradas.Add(l);
        }

        // 2. LÓGICA DEL EVENTO (Solo si NUNCA ha ocurrido antes)
        if (!GLOBAL.TieneFlag("evento_parpadeo_hecho"))
        {
            float dado = Random.Range(0f, 100f);
            
            if (dado <= probabilidad)
            {
                // Lo activamos localmente
                lucesRotas = true;
                
                // Lo marcamos en el guardado GLOBAL para que NO vuelva a pasar
                GLOBAL.PonerFlag("evento_parpadeo_hecho");
                
                StartCoroutine(RutinaParpadeo());
            }
        }
    }

    private IEnumerator RutinaParpadeo()
    {
        if (audioChisporroteo) 
        {
            audioChisporroteo.loop = true;
            audioChisporroteo.Play();
        }

        // Bucle infinito mientras el objeto exista (mientras estemos en la sala)
        while (lucesRotas)
        {
            // A) CAMBIO DE INTENSIDAD (Flicker rápido)
            float intensidadAleatoria = Random.Range(0f, 2.5f);
            
            foreach (Light luz in lucesEncontradas) 
            {
                if(luz != null) luz.intensity = intensidadAleatoria;
            }

            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            
            // B) APAGÓN TOTAL (Probabilidad pequeña del 20%)
            if (Random.value > 0.8f)
            {
                foreach (Light luz in lucesEncontradas)
                {
                    if(luz != null) luz.enabled = false;
                }
                
                yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
                
                foreach (Light luz in lucesEncontradas)
                {
                    if(luz != null) luz.enabled = true;
                }
            }
        }
    }
}