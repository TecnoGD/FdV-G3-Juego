using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Codigo.Scripts
{
    public class GeneradorCombates : MonoBehaviour
    {
        public GameObject luchador;
        private void Start()
        {
            
            var limites = GLOBAL.limitesSpawn[GLOBAL.datosPartida.actoActual - 1];
            var progreso = GLOBAL.datosPartida.progresoHistoria;
            SistemaCombate.boss =GLOBAL.batallasPlanificadas.Contains(progreso);
            var layout = SistemaCombate.boss switch
            {
                true => GLOBAL.instance.layoutsBosses[GLOBAL.batallasPlanificadas.IndexOf(progreso)].luchadores,
                false => GLOBAL.instance.layoutsCombate[Random.Range(limites.Item1, limites.Item2)].luchadores
            };
            var pos = 0;
            for (int i = 0; i < layout.Count; i++)
            {
                var enemigo = Instantiate(luchador, gameObject.transform);
                var vector3 = enemigo.transform.position;
                vector3.x = vector3.x + pos;
                pos += 3;
                enemigo.transform.position = vector3;
                if(layout[i].sprite) enemigo.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = layout[i].sprite;
                
                enemigo.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = layout[i].controller;

                GameObject objetoVisual = enemigo.transform.GetChild(0).gameObject;
                Luchador luchadorBase = objetoVisual.GetComponent<Luchador>();
                if (luchadorBase != null) DestroyImmediate(luchadorBase);

                switch (layout[i].comportamiento)
                {
                    case Comportamiento.Generico:
                        objetoVisual.AddComponent<EnemigoGenerico>();
                        break;
                        
                    case Comportamiento.Bruto:
                        objetoVisual.AddComponent<EnemigoBruto>();
                        break;

                    case Comportamiento.Berserker:
                        objetoVisual.AddComponent<EnemigoBerserker>();
                        break;

                    case Comportamiento.Tactico:
                        objetoVisual.AddComponent<EnemigoTactico>();
                        break;
                }
                var componente = objetoVisual.GetComponent<Luchador>();
                componente.datos = layout[i];
                componente.ActualizarDatos();
            }
        }
    }
}