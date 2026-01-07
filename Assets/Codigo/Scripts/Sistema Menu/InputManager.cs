using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Codigo.Scripts.Sistema_Menu
{
    public class InputManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}