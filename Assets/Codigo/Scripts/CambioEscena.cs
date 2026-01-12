using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    public string escena;
    public Vector3 posicion;
    public bool interactuar;
    public TMP_Text textoInteractuar;
    public BoxCollider boxCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (!interactuar)
        {

            if (other.gameObject.CompareTag("Rig Jugador"))
                GLOBAL.instance.CambiarEscena(escena, posicion);
        }
        else
        {
            textoInteractuar.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(Keyboard.current.fKey.isPressed && other.gameObject.CompareTag("Rig Jugador"))
            GLOBAL.instance.CambiarEscena(escena, posicion);
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactuar)
        {
            textoInteractuar.gameObject.SetActive(false);
        }
    }
}
