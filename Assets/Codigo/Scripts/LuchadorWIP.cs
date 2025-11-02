/*using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Codigo.Scripts.DatosCombate;

namespace Codigo.Scripts
{
    public class LuchadorWIP : MonoBehaviour
    {
        public Estadisticas LuchadorEstadisticas;
        public int vida;
        public bool defiende;
        public string combatName;
        public int listOrder = -1; //Empieza en 0, -1 para seleccion aleatoria
        public Action<List<LuchadorWIP>> LuchadorAI;
        public Animator animator;
        public LuchadorWIP objetivo;
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
            if (defiende)
                dañoRecibido /= 2;
        
            if (dañoRecibido == 0)
                dañoRecibido = 1;
        
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
}*/
