using System;
using System.Collections;
using UnityEngine;

namespace Codigo.Scripts
{
    public class AutoGuardado : MonoBehaviour
    {
        

        private IEnumerator Start()
        {
            yield return null;
            GLOBAL.GuardarProgreso();
        }
    }
}