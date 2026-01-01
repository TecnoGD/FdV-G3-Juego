using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using Codigo.Scripts;
using Codigo.Scripts.Sistema_Menu;

public class TestMovimientoJugador
{
    private GameObject jugadorGO;
    private Jugador jugador;
    private Keyboard teclado;
    private GameObject globalGO;
    private GameObject menuSystemGO;

    // Setup de los tests
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Mock de MenuSystem necesario para GLOBAL.Start()
        menuSystemGO = new GameObject("MenuSystem");
        NewMenuSystem menuSystem = menuSystemGO.AddComponent<NewMenuSystem>();
        NewMenuSystem.Instancia = menuSystem; // Asignamos instancia manualmente
        menuSystem.defaultMenus[0] = new GameObject("MenuJugadorMock").AddComponent<TabMenu>();; // Mock del menú de jugador

        // Creamos el GLOBAL para que Start() de jugador se ejecute sin problemas
        globalGO = new GameObject("GLOBAL");
        globalGO.AddComponent<GLOBAL>();
        yield return null; // Esperamos a que Awake se ejecute
        
        // Desactivamos el combate para que el jugador se pueda mover
        GLOBAL.enCombate = false;
        
        // Limpiamos los datos guardados del juego,IMPORTANTE: no afecta al juego real, es solo para que no interfieran con los tests
        if (GLOBAL.guardado != null)
        {
            if (GLOBAL.guardado.objetosConsumibles != null)
                GLOBAL.guardado.objetosConsumibles.Clear();
                
            // También limpiamos los objetos seleccionados para combate
            if (GLOBAL.guardado.objetosSeleccionadosCombate != null)
            {
                for (int i = 0; i < GLOBAL.guardado.objetosSeleccionadosCombate.Length; i++)
                {
                    GLOBAL.guardado.objetosSeleccionadosCombate[i] = -1;
                }
            }
        }
        
        // Creamos el teclado de prueba
        teclado = InputSystem.AddDevice<Keyboard>();
        
        // Creamos el jugador de prueba (GameObject primero, componente después)
        jugadorGO = new GameObject("JugadorTest");
        
        // Añadimos un Animator simulado para evitar errores en el Start
        jugadorGO.AddComponent<Animator>();
        
        // Creamos el SistemaInteraccion como hijo del jugador para que Start() de jugador se ejecute sin problemas
        GameObject hijo0 = new GameObject("Hijo0"); // Primer hijo (índice 0)
        hijo0.transform.SetParent(jugadorGO.transform);
        
        GameObject sistemaInteraccionGO = new GameObject("SistemaInteraccion"); // Segundo hijo (índice 1)
        sistemaInteraccionGO.transform.SetParent(jugadorGO.transform);
        sistemaInteraccionGO.AddComponent<SistemaInteraccion>();
        
        // Ahora sí añadimos el componente Jugador (esto ejecutará Start())
        jugador = jugadorGO.AddComponent<Jugador>();
        
        yield return null; // Esperamos un frame para que Awake se ejecute
    }
    // Limpiamos la instancia del jugador después de cada test para evitar errores
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Limpiamos las instancias
        Object.Destroy(jugadorGO);
        Object.Destroy(globalGO);
        Object.Destroy(menuSystemGO);
        InputSystem.RemoveDevice(teclado);
        yield return null;
    }

    [UnityTest]
    public IEnumerator JugadorSeMueveDerecha()
    {
        // posicion inicial
        var inicio = jugadorGO.transform.position;
        // Simulamos pulsar la tecla 'D'
        Press(Key.D);
        yield return null;
        Release();
        yield return null;
        // posicion final
        var final = jugadorGO.transform.position;
        Assert.Greater(final.x, inicio.x, "El jugador debería moverse hacia la derecha");
    }

    [UnityTest]
    public IEnumerator JugadorSeMueveIzquierda()
    {
        // posicion inicial
        var inicio = jugadorGO.transform.position;
        // Simulamos pulsar la tecla 'A'
        Press(Key.A);
        yield return null;
        Release();
        yield return null;
        // posicion final
        var final = jugadorGO.transform.position;
        Assert.Less(final.x, inicio.x, "El jugador debería moverse hacia la izquierda");
    }
    private void Press(Key key) => InputSystem.QueueStateEvent(teclado, new KeyboardState(key));
    private void Release() => InputSystem.QueueStateEvent(teclado, new KeyboardState());
}
