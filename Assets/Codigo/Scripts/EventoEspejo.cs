using UnityEngine;
using Codigo.Scripts; // Necesario para GLOBAL, IInteractuable, SistemaDialogo

public class EventoEspejo : MonoBehaviour, IInteractuable
{
    [Header("Referencias Visuales")]
    public GameObject iconoExclamacion;

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

        if (GLOBAL.TieneFlag("espejo_roto"))
        {
            // Si ya estaba roto, desactivamos todo inmediatamente
            DesactivarEspejo();
        }
    }

    // Implementación de la interfaz IInteractuable
    public void Interactuar()
    {
        // Evitar solapamientos si ya se está hablando
        if (SistemaDialogo.instance.enDialogo) return;

        if (GLOBAL.TieneFlag("espejo_roto")) return;

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
        // Quitamos la exclamación ahora que empieza el evento final
        GLOBAL.EnEvento = true;
        if (iconoExclamacion != null)
        {
            iconoExclamacion.SetActive(false);
        }
        // Lanzamos el diálogo con el texto de la ruptura
        SistemaDialogo.instance.IniciarDialogo(dialogoFase2, nombreEntidad, null);
        
        // Esperamos a que el jugador termine de leer
        yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

        GLOBAL.PonerFlag("espejo_roto");

        DesactivarEspejo();
        GLOBAL.EnEvento = false;
        Debug.Log("Espejo roto permanentemente.");
    }

    private void DesactivarEspejo()
    {
        // 1. Quitar exclamación
        if (iconoExclamacion != null) iconoExclamacion.SetActive(false);

        // 2. Desactivar collider para que no se pueda pulsar F
        if (triggerInteraction) triggerInteraction.enabled = false;
    }
}
