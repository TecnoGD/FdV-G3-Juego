using UnityEngine;
using System.Collections;
using Codigo.Scripts;

public class EventoNarrativaActo2 : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreNPC = "Voz Desconocida"; // O "Prisionero Extraño"
    [TextArea] public string[] dialogo;

    IEnumerator Start()
    {
        // Solo se activa en el paso previo al Boss del Acto 2 (que es el 16)
        if (GLOBAL.guardado.progresoHistoria == 15)
        {
            // Esperamos medio segundo para que la escena cargue visualmente
            yield return new WaitForSeconds(0.5f);

            // Lanzamos el diálogo
            if (SistemaDialogo.instance != null)
            {
                SistemaDialogo.instance.IniciarDialogo(dialogo, nombreNPC, null);
                
                // Esperamos a que el jugador termine de leer para que empiece el combate real
                yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);
            }
        }
        else
        {
            // Si no es el momento 15, este script se borra para no molestar
            Destroy(gameObject);
        }
    }
}
