using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Luchador : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int vida;
    public int vidaMax;
    public int ataque;
    public int defensa;
    public int ataqueEspecial;
    public string combatName;
    public int listOrder = -1; //Empieza en 0
    public Action<List<Luchador>> luchadorAI;
    public Animator animator;
    public Luchador objetivo;
    public UnityEvent finTurno;
    
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int RecibeDaño(int dañoRecibido)
    {
        int dañoReal = vida;
        if (vida < dañoRecibido)
        {
            vida = 0;
        }
        else
            vida -= dañoRecibido;
        
        return dañoReal - vida;
    }
    
    public void finAnimacion()
    {
        finTurno.Invoke();
    }
}
