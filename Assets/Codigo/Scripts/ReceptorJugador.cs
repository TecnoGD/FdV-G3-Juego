using UnityEngine;
using System.Collections;
using Codigo.Scripts;

public class ReceptorJugador : MonoBehaviour
{
    // Arrastra aquí el punto donde quieres que aparezca (opcional)
    public Transform puntoAparicion; 

    IEnumerator Start()
    {
        if (GLOBAL.instance.Jugador != null)
        {
            // 1. Colocamos al jugador en su sitio (mientras es invisible)
            if (puntoAparicion != null)
            {
                GLOBAL.instance.Jugador.transform.position = puntoAparicion.position;
                GLOBAL.instance.Jugador.transform.rotation = puntoAparicion.rotation;
            }
            
            // 2. Nos aseguramos de que la cámara se centre YA
            // (Al estar el jugador activo, la cámara ya lo ha encontrado en su Start, 
            //  solo necesitamos que no haga transición suave desde el infinito)
            CamaraSeguimiento cam = Camera.main.GetComponent<CamaraSeguimiento>();
            if (cam != null)
            {
                // Forzamos la posición de la cámara al destino inmediatamente
                cam.transform.position = GLOBAL.instance.Jugador.transform.position + cam.offset;
            }
        }

        // 3. Esperamos el tiempo de "negro/misterio"
        yield return new WaitForSeconds(2.0f);

        // 4. Hacemos visible al jugador
        if (GLOBAL.instance.Jugador != null)
        {
            Renderer[] graficos = GLOBAL.instance.Jugador.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in graficos) r.enabled = true;
        }
    }
}
