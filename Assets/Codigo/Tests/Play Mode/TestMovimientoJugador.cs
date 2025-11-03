using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Codigo.Scripts;

public class TestMovimientoJugador
{
    private GameObject jugadorGO;
    private Jugador jugador;
    // Setup de los tests
    [UnitySetUp]
    public IEnumerator SetUp()
    {
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
        // Limpiamos la instancia
        Object.Destroy(jugadorGO);
        yield return null;
    }

    [UnityTest]
    public IEnumerator JugadorSeMueveDerecha()
    {
        // posicion inicial
        var inicio = jugadorGO.transform.position;
        // mover a la derecha
        jugador.Mover(Vector3.right);
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
        // mover a la izquierda
        jugador.Mover(Vector3.left);
        yield return null;
        // posicion final
        var final = jugadorGO.transform.position;
        Assert.Less(final.x, inicio.x, "El jugador debería moverse hacia la izquierda");
    }
}
