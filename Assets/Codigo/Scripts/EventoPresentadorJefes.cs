using UnityEngine;
using System.Collections;
using Codigo.Scripts;

public class EventoPresentadorJefes : MonoBehaviour
{
    [Header("Configuración General")]
    public string nombrePresentador = "Presentador";

    [Header("Jefe Acto 1 (Historia = 7)")]
    [TextArea] public string[] discursoJefe1;

    [Header("Jefe Acto 2 (Historia = 17)")]
    [TextArea] public string[] discursoJefe2;

    [Header("Jefe Final (Historia = 24)")]
    [TextArea] public string[] discursoJefe3;

    IEnumerator Start()
    {
        int momentoHistoria = GLOBAL.guardado.progresoHistoria;
        string[] dialogoAUsar = null;

        // SELECCIONAR EL DISCURSO SEGÚN EL MOMENTO
        switch (momentoHistoria)
        {
            case 7:
                dialogoAUsar = discursoJefe1;
                break;
            case 17:
                dialogoAUsar = discursoJefe2;
                break;
            case 24:
                dialogoAUsar = discursoJefe3;
                break;
        }

        // SI TOCA JEFE, EJECUTAMOS
        if (dialogoAUsar != null && dialogoAUsar.Length > 0)
        {
            // Esperamos un poco para que la escena cargue visualmente
            yield return new WaitForSeconds(0.5f);

            if (SistemaDialogo.instance != null)
            {
                GLOBAL.EnEvento = true;
                SistemaDialogo.instance.IniciarDialogo(dialogoAUsar, nombrePresentador, null);
                
                // (Opcional) Bloquear hasta que termine de hablar
                yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);
                GLOBAL.EnEvento = false;
            }
        }
        else
        {
            // Si no estamos en ninguno de esos momentos, el script se borra para no molestar
            Destroy(gameObject);
        }
    }
}
