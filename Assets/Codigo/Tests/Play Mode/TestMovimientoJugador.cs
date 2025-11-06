using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using Codigo.Scripts;

public class TestMovimientoJugador
{
    private GameObject jugadorGO;
    private Jugador jugador;
    private Keyboard teclado;
    private GameObject globalGO;
    // Setup de los tests
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Creamos el GLOBAL para que Start() de jugador se ejecute sin problemas
        globalGO = new GameObject("GLOBAL");
        yield return null; // Esperamos a que Awake se ejecute
        globalGO.AddComponent<GLOBAL>();
        // Desactivamos el combate para que el jugador se pueda mover
        GLOBAL.enCombate = false;
        // Creamos el teclado de prueba
        teclado = InputSystem.AddDevice<Keyboard>();
        // Creamos el jugador de prueba
        jugadorGO = new GameObject("JugadorTest");
        jugador = jugadorGO.AddComponent<Jugador>();

        // Añadimos un Animator simulado para evitar errores en el Start
        jugadorGO.AddComponent<Animator>();
        yield return null; // Esperamos un frame inicialización
    }
    // Limpiamos la instancia del jugador después de cada test para evitar errores
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Limpiamos las instancias
        Object.Destroy(jugadorGO);
        Object.Destroy(globalGO);
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
