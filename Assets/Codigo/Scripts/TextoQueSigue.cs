using UnityEngine;


namespace Codigo.Scripts
{
    public class TextoQueSigue: MonoBehaviour
    {
        public Transform aSeguir;

        void Update()
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(aSeguir.position);
            
            // Asigna la posición de pantalla al RectTransform del elemento de UI
            // Es posible que necesites ajustar esto si el anclaje de la UI no está en el centro
            gameObject.transform.position = screenPos;
        }
    }
}