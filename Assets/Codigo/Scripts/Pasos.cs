using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Codigo.Scripts
{
    public class Pasos : MonoBehaviour
    {
        public AudioClip[] clipsPasos;
        [Header("Componentes")]
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void HacePaso()
        {
            var paso = clipsPasos[Random.Range(0, clipsPasos.Length)];
            audioSource.PlayOneShot(paso);
        }
    }
    
    
}