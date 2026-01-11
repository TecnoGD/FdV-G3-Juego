using System;
using UnityEngine;

namespace Codigo.Scripts
{
    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            var config = GLOBAL.Configuracion;
            CambiarResoluciones(config.width, config.height, config.frameRate);
            AlternarPantallaCompleta(config.fullScreen);
            AlternarVSync(config.VSync);
        }

        public static void CambiarResoluciones(int altura, int ancho, double tasaRefresco)
        {
            Screen.SetResolution(ancho, altura, Screen.fullScreen);
            Application.targetFrameRate = (int)tasaRefresco;
            GLOBAL.Configuracion.width =  ancho;
            GLOBAL.Configuracion.height =  altura;
            GLOBAL.Configuracion.frameRate =  (int)tasaRefresco;
            
        }

        public static void AlternarPantallaCompleta(bool estado)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, estado);
            GLOBAL.Configuracion.fullScreen =  estado;
        }

        public static void AlternarVSync(bool estado)
        {
            QualitySettings.vSyncCount = estado ? 1 : 0;
            GLOBAL.Configuracion.VSync =  estado;
        }
        
    }
}