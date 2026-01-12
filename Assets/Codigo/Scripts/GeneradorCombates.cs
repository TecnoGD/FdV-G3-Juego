using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Codigo.Scripts
{
    public class GeneradorCombates : MonoBehaviour
    {
        public GameObject luchador;
        private void Start()
        {
            var layout = GLOBAL.instance.layoutsCombate[Random.Range(0, GLOBAL.instance.layoutsCombate.Count)].luchadores;
            var pos = 0;
            for (int i = 0; i < layout.Count; i++)
            {
                var enemigo  = Instantiate(luchador, gameObject.transform);
                var vector3 = enemigo.transform.position;
                vector3.x = vector3.x + pos;
                pos += 3;
                enemigo.transform.position = vector3;
                switch (layout[i].comportamiento)
                {
                    case Comportamiento.Generico:
                        enemigo.transform.GetChild(0).gameObject.AddComponent<EnemigoGenerico>();
                        break;
                    
                }

                var componente = enemigo.transform.GetChild(0).gameObject.GetComponent<Luchador>();
                componente.datos = layout[i];
                componente.ActualizarDatos();
            }
           
        }
    }
}