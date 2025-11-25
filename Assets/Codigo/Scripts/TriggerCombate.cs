using System;
using Codigo.Scripts;
using UnityEngine;

public class TriggerCombate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Rig Jugador")) return;
        SistemaCombate.instance.gameObject.SetActive(true);
        SistemaCombate.instance.IniciarCombate();
        Destroy(gameObject);
    }
}
