using UnityEngine;
using UnityEngine.SceneManagement;

namespace Codigo.Scripts
{
    public class Jugador : Luchador
    {
        public float velocidad = 5f;
        public Vector3 movimiento;
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
            string escena = SceneManager.GetActiveScene().name;
            if (escena == "SalaDescanso")
                ControlMovimiento();
        }
        private void ControlMovimiento()
        {
            movimiento = Vector3.zero;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                movimiento.x = -1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                movimiento.x = 1;
            
            Mover(movimiento);
        }
        private void Mover(Vector3 movimiento)
        {
            transform.position += movimiento.normalized * (velocidad * Time.deltaTime);
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
