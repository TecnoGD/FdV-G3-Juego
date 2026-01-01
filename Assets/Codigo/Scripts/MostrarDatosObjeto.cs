using TMPro;
using UnityEngine;

namespace Codigo.Scripts
{
    public class MostrarDatosObjeto : MonoBehaviour
    {
        public TMP_Text textoNombre;
        public TMP_Text textoDescripcion;


        void OnEnable()
        {
            textoNombre.text = "";
            textoDescripcion.text = "";
        }
        public void CambiarDatos(ObjetoConsumible objetoConsumible)
        {
            if (objetoConsumible)
            {
                textoNombre.text = objetoConsumible.nombre;
                textoDescripcion.text = objetoConsumible.descripcion;                
            }
            else
            {
                textoNombre.text = "";
                textoDescripcion.text = "";
            }

        }
    }
}