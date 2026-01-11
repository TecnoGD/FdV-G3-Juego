using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Codigo.Scripts.Sistema_Menu
{
    
    public class MenuResoluciones : Menu
    {
        public TMP_Dropdown dropdown;
        public Toggle togglePantallaCompleta;
        public Toggle toggleVSync;
        private Resolution[] _resoluciones;

        public void Start()
        {
            RevisarResoluciones();
            RevisarPantallaCompleta();
            RevisarVSync();
        }

        public void RevisarPantallaCompleta()
        {
            togglePantallaCompleta.isOn = Screen.fullScreen;
            
        }

        public void RevisarVSync()
        {
            toggleVSync.isOn = QualitySettings.vSyncCount == 1;
        }

        public void RevisarResoluciones()
        {
            _resoluciones = Screen.resolutions;
            dropdown.ClearOptions();
            var opciones = new List<string>();
            var resolucionActual = 0;
            for (var i = 0; i < _resoluciones.Length; i++)
            {
                var opcion = _resoluciones[i].width + " x " + _resoluciones[i].height + " " + ((int)_resoluciones[i].refreshRateRatio.value) + "Hz";
                opciones.Add(opcion);

                if (_resoluciones[i].width == Screen.currentResolution.width &&
                    _resoluciones[i].height == Screen.currentResolution.height && ((int)_resoluciones[i].refreshRateRatio.value) == Application.targetFrameRate)
                {
                    resolucionActual = i;
                }
            }
            dropdown.AddOptions(opciones);
            dropdown.SetValueWithoutNotify(resolucionActual);
            dropdown.RefreshShownValue();
        }
        
        public void CambiarResoluciones(int indiceResolucion)
        {
            var resolucion = _resoluciones[indiceResolucion];
            ScreenManager.CambiarResoluciones(resolucion.height, resolucion.width,  resolucion.refreshRateRatio.value);
            NewMenuSystem.MenuAnterior();
        }

        public void AlternarPantallaCompleta(bool estado)
        {
            ScreenManager.AlternarPantallaCompleta(estado);
        }
        
        public void AlternarVsync(bool estado)
        {
            ScreenManager.AlternarVSync(estado);
        }


    }
}