using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : Luchador
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaMax = 20;
        vida = vidaMax;
        ataque = 1;
        //combatName = "Enemigo";
        luchadorAI = turnAI;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void turnAI(List<Luchador> listaLuchadores)
    {
        objetivo = listaLuchadores[0]; 
        animator.Play("AtaqueEnemigo");
    }

    private void ataqueBasico()
    {
        objetivo.RecibeDaño(ataque);
    }
}
