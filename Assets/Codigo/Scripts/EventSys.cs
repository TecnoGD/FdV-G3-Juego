using System;
using UnityEngine;

namespace Codigo.Scripts
{
    public class EventSys : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}