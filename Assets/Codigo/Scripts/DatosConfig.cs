using UnityEngine;

namespace Codigo.Scripts
{
    public class DatosConfig
    {
        public float MasterVolume;
        public float MusicVolume;
        public float SfxVolume;
        public float AmbientVolume;
        public int width;
        public int height;
        public bool fullScreen;
        public bool VSync;
        [SerializeField] public double frameRate;
        

        public DatosConfig()
        {
            MasterVolume = 1f;
            MusicVolume = 1f;
            SfxVolume = 1f;
            AmbientVolume = 1f;
            fullScreen = true;
            VSync = true;
            width = Screen.currentResolution.width;
            height = Screen.currentResolution.height;
            frameRate = 60d;
        }
    }
}