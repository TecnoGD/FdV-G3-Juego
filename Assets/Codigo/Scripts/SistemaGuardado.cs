using System;
using System.IO;
using UnityEngine;

namespace Codigo.Scripts
{
    public class SistemaGuardado
    {
        private static string _path = Application.dataPath + "/datosGuardado.json"; // Directorio del archivo de guardado
        private static string _configPath = Application.dataPath + "/config.json"; // Directorio del archivo de guardado
        
        // Sin implementar
        public static void Guardar(DatosGuardado datosGuardado)
        {
            throw new NotImplementedException();
        }
        

        /* Funcion que carga y devuelve los datos de guardado del juego, en caso de que el archivo no exista o este
           corrupto se genera un nuevo archivo (si esta corrupto se borra el anterior archivo y se sustituye) */
        public static DatosGuardado Cargar()
        {
            
            DatosGuardado guardado = null;
            if (File.Exists(_path))     // Comprueba si el archivo existe
            {
                var contenido =  File.ReadAllText(_path);                       // Se lee el contenido del archivo
                guardado = JsonUtility.FromJson<DatosGuardado>(contenido);            // Se transforma el contenido a un
                                                                                      // formato valido
                if (!ValidarDatosGuardado(guardado))                                  // LLama a metodo para comprobar 
                {                                                                     // la validez de los datos
                    File.Delete(_path);
                    guardado = NuevoArchivoGuardado();                                // Se genera nuevo archivo de guardado
                    Debug.Log("Archivo de Guardado Corrupto, se crea nuevo archivo");
                }
            }
            else
            {
                guardado = NuevoArchivoGuardado();                                    // Se genera nuevo archivo de guardado
                Debug.Log("No existe archivo de guardado, se crea uno");
            }
            guardado.CargarObjetos();

            return guardado;
        }

        public static DatosConfig CargarConfiguracion()
        {
            DatosConfig config = null;
            if (File.Exists(_configPath))     // Comprueba si el archivo existe
            {
                var contenido =  File.ReadAllText(_configPath);                       // Se lee el contenido del archivo
                config = JsonUtility.FromJson<DatosConfig>(contenido);            // Se transforma el contenido a un
                // formato valido
            }
            else
            {
                config = NuevoArchivoConfiguracion();                             // Se genera nuevo archivo de guardado
                Debug.Log("No existe archivo de configuracion, se crea uno");
            }
            return config;
        }

        public static void GuardarConfiguracion(DatosConfig config)
        {
            var json = JsonUtility.ToJson(config, true);
            File.WriteAllText(_configPath, json);
        }

        /* Funcion que comprueba si los datos del archivo de guardado son validos
           POST: - Si en las estadisticas del jugador se encuentra algun valor inferior a 0 identifica corrupcion de datos
                   en caso contrario se devuelve un resultado correcto*/
        private static bool ValidarDatosGuardado(DatosGuardado guardado)
        {
            bool guardadoCorrecto = true;
            DatosCombate.Estadisticas estadisticas = guardado.estadisticasJugador;
            guardadoCorrecto = estadisticas is { ataque: >= 1, defensa: >= 1, ataqueEspecial: >= 1, defensaEspecial: >= 1, vidaMax: >= 1 };
            return guardadoCorrecto;
        }

        /* Función que genera y devuelve un nuevo archivo de guardado*/
        static DatosGuardado NuevoArchivoGuardado()
        {        
            DatosGuardado guardado = new DatosGuardado(GLOBAL.instance.objetivoPrueba);                   //Genera nuevos datos de guardado
            var json = JsonUtility.ToJson(guardado, true);   //Formatea los datos a un formato JSON
            File.WriteAllText(_path, json);                         // Escribe/Genera en el archivo de guardado 
            return guardado;                                                // Devuelve referencia a los datos de guardado 
        }

        static DatosConfig NuevoArchivoConfiguracion()
        {
            DatosConfig config = new DatosConfig();
            var json = JsonUtility.ToJson(config, true);
            File.WriteAllText(_configPath, json);
            return config;
        }
    }
}