using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using Codigo.Scripts; // Para acceder a GLOBAL y SistemaDialogo
using UnityEngine.EventSystems;

public class SecuenciaPrologo : MonoBehaviour
{
    [Header("UI Referencias")]
    public GameObject grupoInputNombre; // El panel/objeto padre donde está el input
    public TMP_InputField inputNombre;  // Donde escribe el jugador
    public Button botonConfirmar;
    
    [Header("Configuración Narrativa")]
    public string nombreEntidad = ""; // La Voz
    
    [TextArea] public string[] dialogoInicial; 
    [TextArea] public string[] dialogoRespuesta;

    [Header("Siguiente Escena")]
    public string nombreEscenaJuego = "SalaB";

    private void Start()
    {
        GLOBAL.EnEvento = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (SistemaDialogo.instance != null)
        {
            SistemaDialogo.instance.usarInputInterno = true;
        }

        if (GLOBAL.instance != null && GLOBAL.instance.Jugador != null)
        {
            GLOBAL.instance.Jugador.gameObject.SetActive(false);
        }

        // Iniciamos la secuencia
        StartCoroutine(RutinaInicio());
    }

    private IEnumerator RutinaInicio()
    {
        // 1. Silencio dramático (2 segundos)
        yield return new WaitForSeconds(2f);
        Debug.Log("Rutina Inicio");

        // 2. Habla La Voz (Parte 1)
        SistemaDialogo.instance.IniciarDialogo(dialogoInicial, nombreEntidad, null);

        // Esperamos a que el jugador termine de leer
        yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

        // 3. Activamos el panel para pedir el nombre
        grupoInputNombre.SetActive(true);
        inputNombre.text = ""; 
        // DAR EL FOCO AUTOMÁTICAMENTE AL TEXTO
        EventSystem.current.SetSelectedGameObject(inputNombre.gameObject);
        inputNombre.ActivateInputField();

        inputNombre.onSubmit.AddListener(delegate { BotonConfirmarNombre(); });
    }

    // ESTA FUNCIÓN SE LLAMARÁ DESDE EL BOTÓN "ACEPTAR" EN EL CANVAS
    public void BotonConfirmarNombre()
    {
        if (string.IsNullOrEmpty(inputNombre.text)) return;

        // A. Guardamos el nombre
        if(GLOBAL.instance != null) // Pequeña protección
        {
             GLOBAL.datosPartida.nombreJugador = inputNombre.text;
             
             GLOBAL.datosPartida.actoActual = 1;
             GLOBAL.datosPartida.progresoHistoria = 0; 
             if (GLOBAL.datosPartida.flagsEventos != null) GLOBAL.datosPartida.flagsEventos.Clear();
        }

        // B. Ocultamos el panel y seguimos
        grupoInputNombre.SetActive(false);
        StartCoroutine(RutinaFinal());
    }

    private IEnumerator RutinaFinal()
    {
        // 4. Preparamos el diálogo final con el nombre
        string[] dialogoProcesado = new string[dialogoRespuesta.Length];
        for (int i = 0; i < dialogoRespuesta.Length; i++)
        {
            dialogoProcesado[i] = string.Format(dialogoRespuesta[i], inputNombre.text);
        }

        // 5. Habla La Voz (Parte 2 - Reacción)
        SistemaDialogo.instance.IniciarDialogo(dialogoProcesado, nombreEntidad, null);

        yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

        // REACTIVAMOS AL JUGADOR ANTES DE VIAJAR
        if (GLOBAL.instance != null && GLOBAL.instance.Jugador != null)
        {
            // 1. Aseguramos que el objeto está ACTIVO (para que la cámara lo detecte)
            GLOBAL.instance.Jugador.gameObject.SetActive(true);

            // 2. Apagamos sus gráficos para que viaje "invisible"
            Renderer[] graficos = GLOBAL.instance.Jugador.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in graficos) r.enabled = false;
        }

        if (SistemaDialogo.instance != null) SistemaDialogo.instance.usarInputInterno = false;

        SceneManager.LoadScene(nombreEscenaJuego);
    }
}