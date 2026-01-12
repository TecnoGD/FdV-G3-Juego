using System.Collections;
using UnityEngine;
using Codigo.Scripts; // Necesario para GLOBAL y SistemaDialogo

public class EventoPasilloMuerte : MonoBehaviour
{
    [Header("Configuración de Historia")]
    public int idEventoRequerido = 5; // Ajusta este número al momento intermedio del Acto 2

    [Header("Atmósfera")]
    public GameObject lucesDelPasillo; // El grupo de luces que se apagarán
    
    [Header("Efecto Ralentización")]
    public float velocidadLenta = 2.0f; // La velocidad reducida (normalmente es 5f)
    private float velocidadOriginal;

    [Header("Narrativa (El Eco)")]
    public string nombreVoz = "???"; // Desconocido para el jugador
    [TextArea] public string[] ecosDelPasado; 

    private bool eventoIniciado = false;

    private void OnTriggerEnter(Collider other)
    {
        // Usamos el tag "Rig Jugador" que usamos en el sistema de cambio de escena
        if (other.CompareTag("Rig Jugador") && !eventoIniciado)
        {
            // Verificamos si estamos en el momento correcto de la historia
            if (GLOBAL.guardado.progresoHistoria == idEventoRequerido)
            {
                StartCoroutine(SecuenciaPesadilla());
            }
        }
    }

    private IEnumerator SecuenciaPesadilla()
    {
        eventoIniciado = true;

        // 1. APAGÓN REPENTINO
        if (lucesDelPasillo) lucesDelPasillo.SetActive(false);
        
        // 2. RALENTIZACIÓN (Efecto "Cámara Lenta/Pesadez")
        // Guardamos la velocidad actual para restaurarla luego
        velocidadOriginal = GLOBAL.instance.Jugador.velocidad; 
        GLOBAL.instance.Jugador.velocidad = velocidadLenta; // Aplicamos la lentitud

        yield return new WaitForSeconds(4.0f); // El jugador camina lento y a oscuras un rato

        // 3. EL ECO (Diálogo)
        SistemaDialogo.instance.IniciarDialogo(ecosDelPasado, nombreVoz, null);
        
        // Esperamos a que el jugador termine de escuchar/leer el eco
        yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

        // 4. RECUPERACIÓN
        // Restauramos la velocidad normal
        GLOBAL.instance.Jugador.velocidad = velocidadOriginal;

        // Restauramos las luces
        if (lucesDelPasillo) lucesDelPasillo.SetActive(true);

        // 5. GUARDAR PROGRESO
        GLOBAL.AumentarProgresoHistoria();
    }
}
