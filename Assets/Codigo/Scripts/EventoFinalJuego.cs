using UnityEngine;
using System.Collections;
using Codigo.Scripts;

public class EventoFinalJuego : MonoBehaviour
{
    [Header("Configuración")]
    public string nombre = "Tétartos Toíchas";
    
    [Header("El Colapso (Historia = 25)")]
    [TextArea] public string[] dialogoFinal;

    IEnumerator Start()
    {
        // 23 era el combate. Al ganar, el sistema suma +1.
        // Por tanto, si estamos en 24, acabamos de matar al Jefe Final.
        if (GLOBAL.guardado.progresoHistoria == 25)
        {
            // Esperamos un poco tras la carga de la escena
            yield return new WaitForSeconds(1.0f);

            if (SistemaDialogo.instance != null)
            {
                // El Presentador explota
                SistemaDialogo.instance.IniciarDialogo(dialogoFinal, nombre, null);
                yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

                // Apagamos al jugador para que no salga en la pantalla de créditos
                if (GLOBAL.instance != null && GLOBAL.instance.Jugador != null)
                {
                    GLOBAL.instance.Jugador.gameObject.SetActive(false);
                }
                Debug.Log("FIN DEL JUEGO");
                UnityEngine.SceneManagement.SceneManager.LoadScene("Creditos");
            }
        }
    }
}