using UnityEngine;

namespace Codigo.Scripts
{
    public class SistemaInteraccion : MonoBehaviour
    {
        private IInteractuable objetoInteractuableActual;  // Guarda el objeto con el que podemos interactuar actualmente
        
        // detectar la tecla F
        public void DetectarInteraccion()
        {
            if (Input.GetKeyDown(KeyCode.F) && objetoInteractuableActual != null)
            {
                objetoInteractuableActual.Interactuar();
            }
        }
    
        // detección de trigger
        private void OnTriggerEnter(Collider other) // 3D physics (usa OnTriggerEnter2D si es 2D)
        {
            // Intentamos obtener el componente que cumple la interfaz IInteractuable
            IInteractuable interactuable = other.GetComponent<IInteractuable>();
        
            if (interactuable != null)
            {
                objetoInteractuableActual = interactuable;
                //Debug.Log("Objeto interactuable detectado: Pulsa F");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IInteractuable interactuable = other.GetComponent<IInteractuable>();
        
            if (interactuable != null && interactuable == objetoInteractuableActual)
            {
                objetoInteractuableActual = null;
                //Debug.Log("Te has alejado del objeto");
            }
        }
    }
}