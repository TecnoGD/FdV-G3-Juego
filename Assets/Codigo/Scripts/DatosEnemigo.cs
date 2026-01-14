using System;
using Codigo.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


    public class DatosEnemigo : MonoBehaviour
    {
        public Luchador luchador;
        public Transform aSeguir;
        public TMP_Text  textoNombre;
        public TMP_Text  textoVida;
        public Image imagenVeneno;
        public Image imagenSangrado;
        public Image imagenAturdido;
        public Slider barra;
        
        void Start()
        {
            
            aSeguir = luchador.gameObject.transform;
            textoNombre.text = luchador.nombre;
            barra.maxValue = luchador.estadisticas.vidaMax;
            gameObject.SetActive(false);
        }
        
        void Update()
        {
            var vector3 = aSeguir.position;
            vector3.y += luchador.gameObject.transform.localScale.y;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(aSeguir.position);
            gameObject.transform.position = screenPos;
            textoVida.text = luchador.vida +  "/" + luchador.estadisticas.vidaMax;
            barra.value = luchador.vida;
            imagenVeneno.gameObject.SetActive(luchador.TieneEstado(Luchador.EstadoAlterado.Veneno));
            imagenSangrado.gameObject.SetActive(luchador.TieneEstado(Luchador.EstadoAlterado.Sangrado));
            imagenAturdido.gameObject.SetActive(luchador.TieneEstado(Luchador.EstadoAlterado.Aturdimiento));
            
            // Asigna la posición de pantalla al RectTransform del elemento de UI
            // Es posible que necesites ajustar esto si el anclaje de la UI no está en el centro
            
        }
    }
