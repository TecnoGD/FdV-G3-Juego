using UnityEngine;
using UnityEngine.EventSystems;

public class MenuFocusInitial : MonoBehaviour
{
    public GameObject botonInicial;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botonInicial);
        Debug.Log("OnEnable ejecutado. Foco puesto en: " + botonInicial.name);
    }
}
