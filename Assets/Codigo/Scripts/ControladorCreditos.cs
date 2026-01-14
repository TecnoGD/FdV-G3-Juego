using UnityEngine;
using System.Collections;

public class ControladorCreditos : MonoBehaviour
{
    [Header("Configuración de Tiempos")]
    public float duracionParte1 = 10.0f; // Tiempo para los textos
    [Header("Referencias a los Canvas")]
    public GameObject canvasTextos;   // El canvas con los dos textos
    public GameObject canvasFinal;    // El canvas que quieres activar después

    void Start()
    {
        // Iniciamos la secuencia nada más cargar la escena
        StartCoroutine(SecuenciaCreditos());
    }

    IEnumerator SecuenciaCreditos()
    {
        if (canvasTextos != null) canvasTextos.SetActive(true);
        if (canvasFinal != null) canvasFinal.SetActive(false);
        yield return new WaitForSeconds(duracionParte1);
        if (canvasTextos != null) canvasTextos.SetActive(false);
        if (canvasFinal != null) canvasFinal.SetActive(true);
    }
}
