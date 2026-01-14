using System;
using System.Collections.Generic;
using UnityEngine;

namespace Codigo.Scripts
{
    [Serializable]
    public class DiccionarioSerializableStringInt: Dictionary<string, int> , ISerializationCallbackReceiver
    {
        
        [SerializeField] private List<string> keys = new List<string>();
	
        [SerializeField] private List<int> values = new List<int>();


        public DiccionarioSerializableStringInt() : base()
        {
        }

        public DiccionarioSerializableStringInt(DiccionarioSerializableStringInt diccionario)
        {
            foreach (var kvp in diccionario)
            {
                Add(kvp.Key, kvp.Value);
            }
        }
	
        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach(KeyValuePair<string, int> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }
	
        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new System.Exception(string.Format(
                    "Hay {0} llaves y {1} valores despues de deserializar. Asegurate de que los tipos de las llaves y valores son serializables."));

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]); 
        }
    }
}