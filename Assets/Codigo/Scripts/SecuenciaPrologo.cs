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
    [TextArea] public string[] dialogoRespuesta; // Recuerda usar {0} para el nombre

    [Header("Siguiente Escena")]
    public string nombreEscenaJuego = "SalaB"; // he cambiado esto a SalaB por defecto

    private void Start()
    {
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
        // Asegúrate de que GLOBAL.guardado existe en tu script GLOBAL.
        // Si te da error aquí, usa GLOBAL.instance.nombreJugador (depende de como tengas tu script GLOBAL)
        if(GLOBAL.instance != null) // Pequeña protección
        {
             // Asumo que tienes una estructura 'guardado' dentro de GLOBAL como en tu código original
             // Si no, cámbialo a donde guardes la variable.
             GLOBAL.guardado.nombreJugador = inputNombre.text;
             
             GLOBAL.guardado.actoActual = 1;
             GLOBAL.guardado.progresoHistoria = 0; 
             if (GLOBAL.guardado.flagsEventos != null) GLOBAL.guardado.flagsEventos.Clear();
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
            // OJO: Aquí uso el texto del input directamente por si GLOBAL tardase en actualizarse
            dialogoProcesado[i] = string.Format(dialogoRespuesta[i], inputNombre.text);
        }

        // 5. Habla La Voz (Parte 2 - Reacción)
        SistemaDialogo.instance.IniciarDialogo(dialogoProcesado, nombreEntidad, null);

        yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

        // REACTIVAMOS AL JUGADOR ANTES DE VIAJAR
        if (GLOBAL.instance != null && GLOBAL.instance.Jugador != null)
        {
            GLOBAL.instance.Jugador.gameObject.SetActive(true);
        }

        if (SistemaDialogo.instance != null)
        {
            SistemaDialogo.instance.usarInputInterno = false;
        }

        // 6. Cargar la siguiente escena
        SceneManager.LoadScene(nombreEscenaJuego);
    }
}