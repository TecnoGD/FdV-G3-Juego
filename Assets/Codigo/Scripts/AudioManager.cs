using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Codigo.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        [SerializeField] AudioMixer mixer;
        
        public const string MIXER_MASTER = "MasterVolume";
        public const string MIXER_MUSIC = "MusicVolume";
        public const string MIXER_SFX = "SfxVolume";
        public const string MIXER_Ambient = "AmbientVolume";

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
            var configuracion = GLOBAL.Configuracion;
            var value = configuracion.MasterVolume;
            mixer.SetFloat(MIXER_MASTER, Mathf.Log10(value) * 20);
            
            value = configuracion.MusicVolume;
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
            
            value = configuracion.SfxVolume;
            mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
            
            value = configuracion.AmbientVolume;
            mixer.SetFloat(MIXER_Ambient, Mathf.Log10(value) * 20);
        }
        
    }
}