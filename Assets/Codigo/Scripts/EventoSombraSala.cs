using System.Collections;
using UnityEngine;
using Codigo.Scripts;

public class EventoSombraSala : MonoBehaviour
{
    [Header("Configuración")]
    public int idEventoRequerido = 2; // Momento en que ocurre (Transición Acto 3)

    [Header("Atmósfera")]
    public GameObject lucesNormales; // Grupo de luces cálidas
    public GameObject lucesMoradas;  // Grupo de luces tenues/moradas
    
    [Header("El Susto (Sombra)")]
    public Transform objetoSombra;   // El objeto que proyectará la sombra (o la silueta negra)
    public Transform puntoInicial;   // Dónde empieza la sombra
    public Transform puntoFinal;     // Dónde termina (se esconderá)
    public float velocidadSombra = 5f; // Un poco más rápido para que sea fugaz

    // HE BORRADO LAS VARIABLES DE "MONÓLOGO" Y "NOMBRE" PORQUE YA NO SE USAN

    private void Start()
    {
        // CASO 1: Ocurre el evento ahora (Primera vez)
        if (GLOBAL.guardado.progresoHistoria == idEventoRequerido)
        {
            StartCoroutine(SecuenciaSilenciosa());
        }
        // CASO 2: Ya pasó (mantener ambiente morado siempre)
        else if (GLOBAL.guardado.progresoHistoria > idEventoRequerido)
        {
            AplicarAmbienteMorado();
        }
        else
        {
            // CASO 3: Aún no ha llegado el momento
            AplicarAmbienteNormal();
        }
    }

    private IEnumerator SecuenciaSilenciosa()
    {
        // 1. Inicio normal (el jugador entra y todo parece bien)
        AplicarAmbienteNormal();
        
        // Preparamos la sombra invisible en su salida
        objetoSombra.position = puntoInicial.position;
        if(objetoSombra) objetoSombra.gameObject.SetActive(true);

        // Esperamos un poco para que el jugador se confíe
        yield return new WaitForSeconds(1.5f); 

        // 2. CAMBIO DE LUCES (El quiebre visual)
        if(lucesNormales) lucesNormales.SetActive(false);
        if(lucesMoradas) lucesMoradas.SetActive(true);

        // 3. MOVIMIENTO DE LA SOMBRA (Fugaz)
        while (Vector3.Distance(objetoSombra.position, puntoFinal.position) > 0.1f)
        {
            objetoSombra.position = Vector3.MoveTowards(objetoSombra.position, puntoFinal.position, velocidadSombra * Time.deltaTime);
            yield return null; 
        }
        
        if(objetoSombra) objetoSombra.gameObject.SetActive(false); // La sombra desaparece al llegar

        // 4. PAUSA DRAMÁTICA (Silencio)
        // En vez de texto, dejamos 2 segundos de silencio para que el jugador asimile el cambio
        yield return new WaitForSeconds(2.0f);

        // 5. FIN Y GUARDADO
        // El ambiente se queda morado y el juego continúa sin decir nada
        GLOBAL.guardado.progresoHistoria++;
    }

    private void AplicarAmbienteNormal()
    {
        if(lucesNormales) lucesNormales.SetActive(true);
        if(lucesMoradas) lucesMoradas.SetActive(false);
        if(objetoSombra) objetoSombra.gameObject.SetActive(false);
    }

    private void AplicarAmbienteMorado()
    {
        if(lucesNormales) lucesNormales.SetActive(false);
        if(lucesMoradas) lucesMoradas.SetActive(true);
        if(objetoSombra) objetoSombra.gameObject.SetActive(false);
    }
}