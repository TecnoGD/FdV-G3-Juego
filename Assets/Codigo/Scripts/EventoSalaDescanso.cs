using UnityEngine;
using System.Collections;
using Codigo.Scripts;

public class EventoSalaDescanso : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreVoz = "La Voz"; 
    public GameObject npcEnEscena; 
    
    [Header("Diálogos")]
    [TextArea] public string[] dialogoEntrada; // Solo lo que se dice AUTOMÁTICAMENTE al entrar
    // [TextArea] public string[] dialogoNPC; <--- ESTO LO BORRAMOS, YA ESTÁ EN EL NPC

    private string idEvento = "SalaDescanso_Visitada";

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2.5f);

        if (!GLOBAL.guardado.flagsEventos.Contains(idEvento))
        {
            if (npcEnEscena != null && GLOBAL.instance.Jugador != null)
            {
                // Hacemos que nos mire al entrar
                npcEnEscena.transform.LookAt(GLOBAL.instance.Jugador.transform);
            }

            SistemaDialogo.instance.IniciarDialogo(dialogoEntrada, nombreVoz, null);
            
            yield return new WaitUntil(() => !SistemaDialogo.instance.enDialogo);

            GLOBAL.guardado.flagsEventos.Add(idEvento);
        }
    }
}