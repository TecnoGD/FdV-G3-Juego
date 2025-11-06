using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Codigo.Scripts
{
    // ScriptableObject para definir y crear ataque con mayor facilidad
    public class Accion : ScriptableObject
    {
        public int Id;                      // ID de la accion, (sin uso)
        public string Nombre;               // Nombre de la accion
        public string Descripcion;          // Descripcion de la accion
        public int EstiloSeleccionObjetivo; // Estilo de seleccion de objetivo, usar constantes de Estilo de Objetivos
        public const int SINOBJETIVO = -1, MONOOBJETIVO = 0, MULTIOBJETIVO = 1, TODOSENEMIGOS = 2; //
        public int numObjetivos;            // Número máximo de objetivos
        
        /* Metodo virtual (se puede hacer override para cambiar su comportamiento) el cual contiene el comportamiento
           de la accion a la hora de iniciar su ejecución, obtiene al luchador que lo invoca y su animator para iniciar
           animaciones */
        public virtual void Ejecuta(Luchador self, Animator animator) { return; }
        
        /* Función virtual (se puede hacer override para cambiar su comportamiento) que devuelve la potencia de la
           accion, dependiendo del diseño del ataque, dependiendo de cuantas veces se haya pedido el valor, la potencia
           devuelta puede cambiar */
        public virtual int ObtenerPotencia(int iteracion) { return 0; }
        
        public virtual int ObtenerTipo() { return -1; }
            
    }
}