using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Codigo.Scripts
{
    public class CombatEsentials : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        public TMP_Text textoVidaJugador, textoVidaEnemigo, textoTurno;
        public Jugador jugador;
        private List<Luchador> _listaLuchadores = new List<Luchador>();
        public GameObject uiJugador;
        public GameObject uiCombate;
        private int _turno;
    
    
        void Start()
        {
            _turno = -1;
            jugador = GameObject.FindGameObjectWithTag("Luchador Jugador").GetComponent<Jugador>();
            jugador.finTurno.AddListener(SiguienteTurno);
            _listaLuchadores.Add(jugador);
            // _listaLuchadores.Insert(); Tal vez para insertar en posiciones de forma forzada
            GameObject[] e = GameObject.FindGameObjectsWithTag("Luchador Enemigo");
            foreach (GameObject g in e)
            {
                Luchador l = g.GetComponent<Luchador>();
                if(l.listOrder == -1)
                    _listaLuchadores.Add(l);
                //else
                //_listaLuchadores.Insert(l.listOrder+1, l);
            
            
                l.finTurno.AddListener(SiguienteTurno);
            }
            Debug.Log(_listaLuchadores.Count);
            SiguienteTurno();
        
        }

        // Update is called once per frame
        void Update()
        {
            textoVidaJugador.text = "Vida: " + jugador.vida;
            textoVidaEnemigo.text = "Vida E: " + _listaLuchadores[1].vida;
        
        }
    
        public void JugadorAccion(int test)
        {
            switch (test)
            {
                case 0:
                    DesactivaUIJugador();
                    jugador.JugadorAtacaObjetivo(_listaLuchadores[1]);
                    jugador.animator.Play("Ataque");
                    //StartCoroutine(EsperarAnimación(jugador.animator));
                    break;
                case 1:
                    DesactivaUIJugador();
                    jugador.JugadorDefiende();
                    break;
                case 2:
                    DesactivaUIJugador();
                    SiguienteTurno();
                    break;
                default:
                    break;
            }
        }

        public void SiguienteTurno()
        {
            Debug.Log("Numero Enemigos" + _listaLuchadores.Count);
            bool muerto = jugador.vida == 0;
            if (muerto || _listaLuchadores.Count == 1)
            {
                FinCombate(muerto);
                return;
            }

            _turno++;
            if(_turno > _listaLuchadores.Count-1)
                _turno = 0;
        
            textoTurno.text = "Turno: " + _listaLuchadores[_turno].combatName;
        
            if (_turno == 0)
            {
                jugador.JugadorResetAtributos();
                ActivaUIJugador();
            }
            else
            {
                EnemigoTurno(_turno);
            }
        
            if (_listaLuchadores[_turno].vida == 0)
            {
                Destroy(_listaLuchadores[_turno].gameObject);
                _listaLuchadores.RemoveAt(_turno);
                _turno--;
                SiguienteTurno();
            }
        }

        private void FinCombate(bool mueres)
        {
            if (mueres)
            {
                textoTurno.text = "Has Perdido!";
            }
            else
            {
                textoTurno.text = "Has Ganado!";
            }
            _listaLuchadores.ForEach(delegate(Luchador l){ Destroy(l.gameObject); });
            _listaLuchadores.Clear();
            GameObject.FindGameObjectWithTag("UI Combate").SetActive(false);
            gameObject.SetActive(false);
        }

        void EnemigoTurno(int turno)
        {
            _listaLuchadores[turno].LuchadorAI(_listaLuchadores);
            //StartCoroutine(EsperarAnimación(enemigo.animator));
        }

        private void DesactivaUIJugador()
        {
            uiJugador.SetActive(false);
            //uiCombate.SetActive(false);
        }
        
        private void ActivaUIJugador()
        {
            uiJugador.SetActive(true);
            //uiCombate.SetActive(true);
        }

        public IEnumerator EsperarAnimación(Animator animator)
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0));;
            SiguienteTurno();
        }
    
    

    }
}
