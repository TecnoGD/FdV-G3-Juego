using UnityEngine;

namespace Codigo.Scripts
{
    public class Jugador : Luchador
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            this.combatName = "Jugador";
            this.vidaMax = 20;
            this.vida = vidaMax;
            this.ataque = 5;
            animator = GetComponent<Animator>();
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void JugadorResetAtributos()
        {
            this.defiende = false;
        }

        public void JugadorAtacaObjetivo(Luchador objetivo)
        {
            this.objetivo = objetivo; 
        
        }
    
        public void JugadorDefiende()
        {
            this.defiende = true;
            finTurno.Invoke();
        }
    
        private void AtaqueBasico()
        {
            objetivo.RecibeDaño(ataque);
        }

    
    }
}
