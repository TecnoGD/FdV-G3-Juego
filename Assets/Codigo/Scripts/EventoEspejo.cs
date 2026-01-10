using UnityEngine;
using Codigo.Scripts; // Necesario para GLOBAL, IInteractuable, SistemaDialogo

public class EventoEspejo : MonoBehaviour, IInteractuable
{
    [Header("Narrativa - Fase 1: Introspección")]
    [TextArea] public string[] dialogoFase1; 

    [Header("Narrativa - Fase 2: La Ruptura")]
    public string nombreEntidad = "Tétartos Toíchas"; // La Voz
    [TextArea] public string[] dialogoFase2; // Incluye la descripción de la ruptura
    
    // Variable interna
    private int estadoInteraccion = 0;
    private Collider triggerInteraction;

    private void Start()
    {
        triggerInteraction = GetComponent<Collider>();
    }

    // Implementación de la interfaz IInteractuable
    public void Interactuar()
    {
        // Evitar solapamientos si ya se está hablando
        if (SistemaDialogo.instance.enDialogo) return;

        if (estadoInteraccion == 0)
        {
            // FASE 1: El jugador se mira y duda
            // Nombre vacío "" para indicar pensamiento interno
            SistemaDialogo.instance.IniciarDialogo(dialogoFase1, "", null); 
            estadoInteraccion++; // Preparamos la siguiente fase
        }
        else if (estadoInteraccion == 1)
        {
            // FASE 2: La Voz interviene y "rompe" el espejo narrativamente
            StartCoroutine(SecuenciaFinal());
        }
    }

    private System.Collections.IEnumerator SecuenciaFinal()
    {
        // 1. Lanzamos el diálogo con el texto de la ruptura
        SistemaDialogo.instance.IniciarDialogo(dialogoFase2, nombreEntidad, null);
        
        // 2. Esperamos a que el jugador termine de leer
        yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

        // 3. "Rompemos" la interacción
        estadoInteraccion++; 
        
        // Desactivamos el collider. El jugador ya no podrá interactuar más.
        // El espejo sigue ahí visualmente, pero para el sistema de juego es como una pared normal.
        if (triggerInteraction) triggerInteraction.enabled = false;
        
        Debug.Log("Evento Espejo finalizado. Interacción desactivada.");
    }
}
