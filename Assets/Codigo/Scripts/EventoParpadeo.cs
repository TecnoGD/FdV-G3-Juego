using System.Collections;
using UnityEngine;
using Codigo.Scripts;

public class EventoParpadeo : MonoBehaviour
{
    [Header("Configuración")]
    [Range(0, 100)] public float probabilidad = 100f; 
    
    [Header("Referencias Visuales")]
    public Light[] lucesAfectadas; 
    public AudioSource audioChisporroteo; 

    // Esta variable ahora es privada, no se guarda en el archivo
    private bool lucesRotas = false; 

    private void Start()
    {
        // Solo intentamos el susto si NUNCA ha ocurrido antes en esta partida
        if (!GLOBAL.guardado.eventoParpadeoYaOcurrio)
        {
            float dado = Random.Range(0f, 100f);
            
            if (dado <= probabilidad)
            {
                // 1. Lo activamos localmente
                lucesRotas = true;
                
                // 2. Lo marcamos en el guardado GLOBAL para que NO vuelva a pasar en el futuro
                GLOBAL.guardado.eventoParpadeoYaOcurrio = true;
                
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
            float intensidadAleatoria = Random.Range(0f, 2.5f);
            foreach (Light luz in lucesAfectadas) if(luz) luz.intensity = intensidadAleatoria;

            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            
            if (Random.value > 0.8f)
            {
                foreach (Light luz in lucesAfectadas) if(luz) luz.enabled = false;
                yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
                foreach (Light luz in lucesAfectadas) if(luz) luz.enabled = true;
            }
        }
    }
}