using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Codigo.Scripts.Sistema_Menu
{
    public class MenuRecompensas : Menu
    {
        public TMP_Text dineroTexto;
        public TMP_Text textoTituloAccion;
        public TMP_Text accionAprendidaTexto;
        public List<int> accionParaAprender;
        
        public override void AccionPorDefecto()
        {
            var dinero = (int)(SistemaCombate.instance.factorDinero * Random.Range(1.0f, 1.5f));
            GLOBAL.instance.Jugador.dinero += dinero;
            dineroTexto.text = dinero + "$";
            var repetidos = new List<Estadistica>();
            var _textos = new[] { "VidaMax", "Ataque", "Defensa", "A.Especial", "D.Especial", "Velocidad" };
            foreach (var seleccionable in seleccionables)
            {
                var mejora = seleccionable.gameObject.GetComponent<MejorarEstadística>();
                var pendiente = true;
                while (pendiente)
                {
                    var estadistica = (Estadistica)Random.Range(0, 5);
                    if (!repetidos.Contains(estadistica))
                    {
                        repetidos.Add(estadistica);
                        mejora.estadisticaAMejorar = estadistica;
                        pendiente = false;
                    }
                }
                    
                mejora.cantidad = (int)(Random.Range(0.5f, 1.1f) * SistemaCombate.instance.factorRecompensa);
                if (mejora.textoBoton)
                    mejora.textoBoton.text = _textos[(int)mejora.estadisticaAMejorar] + " +" + mejora.cantidad;
            }

            var accion = -1;
            if(GLOBAL.datosPartida.progresoHistoria <  accionParaAprender.Count)
                accion = accionParaAprender[GLOBAL.datosPartida.progresoHistoria];
            
            if (accion >= 0 && accion < GLOBAL.acciones.Count)
            {
                textoTituloAccion.gameObject.SetActive(true);
                accionAprendidaTexto.gameObject.SetActive(true);
                GLOBAL.instance.Jugador.accionesJugador.Add(accion);
                accionAprendidaTexto.text = GLOBAL.acciones[accion].Nombre;
            }
            else
            {
                textoTituloAccion.gameObject.SetActive(false);
                accionAprendidaTexto.gameObject.SetActive(false);
            }
            
        }
    }
}