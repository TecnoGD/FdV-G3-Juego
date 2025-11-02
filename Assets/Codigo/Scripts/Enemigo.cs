/*using System.Collections.Generic;
using UnityEngine;
using static Codigo.Scripts.DatosCombate;

namespace Codigo.Scripts
{
    public class Enemigo : LuchadorWIP
    {
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            LuchadorEstadisticas = new Estadisticas(20,1,1,1,1);
            //combatName = "Enemigo";
            vida = LuchadorEstadisticas.vidaMax;
            LuchadorAI = TurnAI;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void TurnAI(List<LuchadorWIP> listaLuchadores)
        {
            objetivo = listaLuchadores[0]; 
            animator.Play("AtaqueEnemigo");
        }

        private void AtaqueBasico()
        {
            objetivo.RecibeDaño(LuchadorEstadisticas.ataque);
        }
    }
}*/
