using UnityEngine;
using System.Collections;
using Codigo.Scripts; 

public class EventoPrimerCombate : MonoBehaviour
{
    [Header("Parte 1: La Voz")]
    public string nombreVoz = "";
    [TextArea] public string[] frasesVoz;

    [Header("Parte 2: El Presentador")]
    public string nombrePresentador = "Presentador"; // O el nombre que quieras
    [TextArea] public string[] frasesPresentador;

    IEnumerator Start()
    {
        // SOLO SI ES LA PRIMERA VEZ (Progreso 0)
        if (GLOBAL.guardado.progresoHistoria == 0)
        {
            GLOBAL.EnEvento = true;
            yield return new WaitForSeconds(0.5f);

            // CONVERSACIÓN 1: LA VOZ
            if (SistemaDialogo.instance != null)
            {
                SistemaDialogo.instance.IniciarDialogo(frasesVoz, nombreVoz, null);
                
                // IMPORTANTE: El script se congela aquí hasta que cierres el cuadro de texto
                yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);
            }

            // Una pausa pequeñita entre uno y otro (0.2 segundos) para que respire
            yield return new WaitForSeconds(0.2f);

            // CONVERSACIÓN 2: EL PRESENTADOR
            if (SistemaDialogo.instance != null)
            {
                SistemaDialogo.instance.IniciarDialogo(frasesPresentador, nombrePresentador, null);

                // Esperamos a que termine también el presentador
                yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);
            }
            GLOBAL.EnEvento = false;
        }
        else
        {
            // Si no es el primer combate, borramos esto
            Destroy(gameObject);
        }
    }
}