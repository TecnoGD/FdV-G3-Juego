using UnityEngine;
using Codigo.Scripts; // Necesario para GLOBAL

public class ControladorLucesDinamicas : MonoBehaviour
{
    [Header("Configuración Básica")]
    [Tooltip("Pon esta etiqueta a todas las Point/Spot lights que quieras que cambien.")]
    public string etiquetaLuces = "Luz Ambiente";

    [Header("--- ACTO 2 (El Declive) ---")]
    [Tooltip("Color hacia el que cambian las luces (Naranja apagado)")]
    public Color colorActo2 = new Color(1.0f, 0.7f, 0.5f); // Naranja suave
    [Range(0f, 1f)] 
    [Tooltip("Porcentaje de intensidad respecto al original (Ej: 0.7 es 70%)")]
    public float multiplicadorIntensidadActo2 = 0.7f;

    [Header("--- ACTO 3 (El Final) ---")]
    [Tooltip("Color hacia el que cambian las luces (Morado oscuro)")]
    public Color colorActo3 = new Color(0.4f, 0.2f, 0.8f); // Morado
    [Range(0f, 1f)] 
    [Tooltip("Porcentaje de intensidad respecto al original (Ej: 0.3 es 30%)")]
    public float multiplicadorIntensidadActo3 = 0.3f;

    void Start()
    {
        // 1. OBTENER PROGRESO
        int historia = 0;
        if (GLOBAL.instance != null)
        {
            historia = GLOBAL.guardado.progresoHistoria;
        }

        // Si estamos en el Acto 1 (historia < 8), no hacemos nada.
        // Las luces se quedan tal y como las diseñaste en la escena.
        if (historia < 8) return;

        // 2. BUSCAR LUCES
        GameObject[] objetosLuz = GameObject.FindGameObjectsWithTag(etiquetaLuces);

        if (objetosLuz.Length == 0)
        {
            Debug.LogWarning("No se encontraron luces con la etiqueta: " + etiquetaLuces);
            return;
        }

        // 3. APLICAR CAMBIOS
        foreach (GameObject obj in objetosLuz)
        {
            Light luz = obj.GetComponent<Light>();

            if (luz != null)
            {
                if (historia >= 17) // ACTO 3
                {
                    // Cambiamos al color morado
                    luz.color = colorActo3;
                    // Bajamos la intensidad mucho (ej: al 30% de lo que tenía)
                    luz.intensity *= multiplicadorIntensidadActo3;
                }
                else if (historia >= 8) // ACTO 2
                {
                    // Cambiamos al color naranja apagado
                    luz.color = colorActo2;
                    // Bajamos la intensidad un poco (ej: al 70% de lo que tenía)
                    luz.intensity *= multiplicadorIntensidadActo2;
                }
            }
        }
    }
}
