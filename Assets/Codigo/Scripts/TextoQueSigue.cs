using UnityEngine;


namespace Codigo.Scripts
{
    public class TextoQueSigue: MonoBehaviour
    {
        public Transform aSeguir;
        Vector3 posicion = new Vector3(0,0,0);
        public int offset = 1;

        void Update()
        {
            if (!aSeguir) return;
            posicion = aSeguir.position;
            posicion.y += offset;
            posicion = Camera.main.WorldToScreenPoint(posicion);
            gameObject.transform.position = posicion;
            // Asigna la posición de pantalla al RectTransform del elemento de UI
            // Es posible que necesites ajustar esto si el anclaje de la UI no está en el centro

        }
    }
}