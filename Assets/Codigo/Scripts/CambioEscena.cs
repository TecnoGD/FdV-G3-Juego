using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    public string escena;
    public Vector3 posicion;

    private void OnTriggerEnter(Collider other)
    {
        GLOBAL.instance.CambiarEscena(escena, posicion);
    }
}
