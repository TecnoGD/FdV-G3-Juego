using UnityEngine;
using System.Collections;
using Codigo.Scripts; 

public class TutorialSalaB : MonoBehaviour
{
    [Header("Configuración Narrativa")]
    public string nombreVoz = "La Voz";
    
    [Header("Control de Escena")]
    public GameObject puertaSalida; 
    public GameObject objetoParaInteractuar; 

    [Header("Diálogos")]
    [TextArea] public string[] faseBienvenida; 
    [TextArea] public string[] faseMovimiento; 
    [TextArea] public string[] faseReaccionMovimiento; 
    [TextArea] public string[] faseInteraccion; 

    // Nombre clave para guardar en la lista de eventos
    private string idEvento = "TutorialSalaB_Completado";

    private void Start()
    {
        if (GLOBAL.datosPartida.flagsEventos.Contains(idEvento))
        {
            if (objetoParaInteractuar != null) objetoParaInteractuar.SetActive(true);

            Destroy(gameObject); 
            return; 
        }

        StartCoroutine(SecuenciaTutorial());
    }

    private IEnumerator SecuenciaTutorial()
    {
        if (puertaSalida != null) puertaSalida.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        // FASES DE DIALOGO
        SistemaDialogo.instance.IniciarDialogo(faseBienvenida, nombreVoz, null);
        yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

        SistemaDialogo.instance.IniciarDialogo(faseMovimiento, nombreVoz, null);
        yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

        // ESPERAR ENTER
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        
        yield return new WaitForSeconds(0.2f);

        SistemaDialogo.instance.IniciarDialogo(faseReaccionMovimiento, nombreVoz, null);
        yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

        if(objetoParaInteractuar != null) objetoParaInteractuar.SetActive(true);

        SistemaDialogo.instance.IniciarDialogo(faseInteraccion, nombreVoz, null);
        yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

        // FIN DEL TUTORIAL
        if (puertaSalida != null) puertaSalida.SetActive(false);

        if (!GLOBAL.datosPartida.flagsEventos.Contains(idEvento))
        {
            GLOBAL.datosPartida.flagsEventos.Add(idEvento);
        }
        GLOBAL.EnEvento = false;
    }
}