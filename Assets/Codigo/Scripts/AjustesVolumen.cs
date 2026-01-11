using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class AjustesVolumen : MonoBehaviour
    {
        [SerializeField] AudioMixer mixer;
        [SerializeField] Slider masterSlider;
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;
        [SerializeField] Slider ambientSlider;
        
        public TMP_Text masterVolumeText;
        public TMP_Text musicVolumeText;
        public TMP_Text sfxVolumeText;
        public TMP_Text ambientVolumeText;
        

        void OnEnable()
        {
            var test = 1f;
            mixer.GetFloat(AudioManager.MIXER_MASTER, out test);
            masterSlider.value = Mathf.Pow(10.0f, test/20.0f);
            masterVolumeText.text = ((int)(masterSlider.value*100)) + "%";
            
            mixer.GetFloat(AudioManager.MIXER_MUSIC, out test);
            musicSlider.value = Mathf.Pow(10.0f, test/20.0f);
            musicVolumeText.text = ((int)(musicSlider.value*100)) + "%";
            
            mixer.GetFloat(AudioManager.MIXER_SFX, out test);
            sfxSlider.value = Mathf.Pow(10.0f, test/20.0f);
            sfxVolumeText.text = ((int)(sfxSlider.value*100)) + "%";
            
            mixer.GetFloat(AudioManager.MIXER_Ambient, out test);
            ambientSlider.value = Mathf.Pow(10.0f, test/20.0f);
            ambientVolumeText.text = ((int)(ambientSlider.value*100)) + "%";
            
        }

        public void SetMasterVolume(float value)
        {
            mixer.SetFloat(AudioManager.MIXER_MASTER, Mathf.Log10(value) * 20);
            masterVolumeText.text = ((int)(value*100)) + "%";
            GLOBAL.Configuracion.MasterVolume =  value;
        }

        public void SetMusicVolume(float value)
        {
            mixer.SetFloat(AudioManager.MIXER_MUSIC, Mathf.Log10(value) * 20);
            musicVolumeText.text = ((int)(value*100)) + "%";
            GLOBAL.Configuracion.MusicVolume = value;
        }

        public void SetSfxVolume(float value)
        {
            mixer.SetFloat(AudioManager.MIXER_SFX, Mathf.Log10(value) * 20);
            sfxVolumeText.text = ((int)(value*100)) + "%";
            GLOBAL.Configuracion.SfxVolume = value;
        }

        public void SetAmbientVolume(float value)
        {
            mixer.SetFloat(AudioManager.MIXER_Ambient, Mathf.Log10(value) * 20);
            ambientVolumeText.text = ((int)(value*100)) + "%";
            GLOBAL.Configuracion.AmbientVolume = value;
        }
    }
}