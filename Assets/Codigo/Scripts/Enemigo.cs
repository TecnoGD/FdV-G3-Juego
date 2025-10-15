using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    public class Enemigo : Luchador
    {
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            vidaMax = 20;
            vida = vidaMax;
            ataque = 2;
            //combatName = "Enemigo";
            LuchadorAI = TurnAI;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void TurnAI(List<Luchador> listaLuchadores)
        {
            objetivo = listaLuchadores[0]; 
            animator.Play("AtaqueEnemigo");
        }

        private void AtaqueBasico()
        {
            objetivo.RecibeDaño(ataque);
        }
    }
}
