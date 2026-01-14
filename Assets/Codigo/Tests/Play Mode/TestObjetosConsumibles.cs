using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Codigo.Scripts;

public class TestObjetosConsumibles
{
    private GameObject jugadorGO;
    private Luchador jugador;

    [SetUp]
    public void Setup()
    {
        // Creamos al Jugador
        jugadorGO = new GameObject("JugadorTest");
        jugador = jugadorGO.AddComponent<Luchador>();
        jugador.vida = 100;
        jugador.estadisticas = new DatosCombate.Estadisticas(10, 10, 10, 10, 10);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(jugadorGO);
    }

    [Test]
    public void PocionRestauraVida()
    {
        // Creamos la poción con un valor de curación de 50 y target Solo Jugador
        var pocion = ScriptableObject.CreateInstance<ObjetoCurativo>();
        pocion.valorCurativo = 50;
        pocion.estiloSeleccionObjetivo = ObjetoConsumible.SOLOJUGADOR;

        // Ejecutamos el objeto sobre el jugador
        pocion.Ejecutar(new List<Luchador> { jugador });

        // Verificamos que la vida ha subido a 150 (100 + 50)
        Assert.AreEqual(150, jugador.vida, "El jugador debería recuperar vida al usar la poción.");
    }

    [Test]
    public void BombaHaceDanoMultiplesObjetivos()
    {
        // Creamos dos enemigos para probar el daño en área
        GameObject enemigo1GO = new GameObject("Enemigo1");
        Luchador enemigo1 = enemigo1GO.AddComponent<Luchador>();
        enemigo1.vida = 100;
        enemigo1.estadisticas = new DatosCombate.Estadisticas(10, 10, 10, 10, 10);

        GameObject enemigo2GO = new GameObject("Enemigo2");
        Luchador enemigo2 = enemigo2GO.AddComponent<Luchador>();
        enemigo2.vida = 100;
        enemigo2.estadisticas = new DatosCombate.Estadisticas(10, 10, 10, 10, 10);

        var objetivosEnemigos = new List<Luchador> { enemigo1, enemigo2 };

        // Creamos la bomba con daño 40 y target Todos Enemigos
        var bomba = ScriptableObject.CreateInstance<ObjetoDanino>();
        bomba.valorDaño = 40;
        bomba.estiloSeleccionObjetivo = ObjetoConsumible.TODOSENEMIGOS;

        // Ejecutamos la bomba sobre los enemigos
        bomba.Ejecutar(objetivosEnemigos);

        // Verificamos que AMBOS han recibido daño
        Assert.AreEqual(60, enemigo1.vida, "El enemigo 1 debería recibir daño por la bomba.");
        Assert.AreEqual(60, enemigo2.vida, "El enemigo 2 debería recibir daño por la bomba.");

        // Limpieza de enemigos
        Object.DestroyImmediate(enemigo1GO);
        Object.DestroyImmediate(enemigo2GO);
    }

    [Test]
    public void ObjetoVenenoHaceDano()
    {
        // Creamos un enemigo
        GameObject enemigoGO = new GameObject("EnemigoVeneno");
        Luchador enemigo = enemigoGO.AddComponent<Luchador>();
        enemigo.vida = 100;
        enemigo.estadisticas = new DatosCombate.Estadisticas(10, 10, 10, 10, 10);
        
        // Creamos el veneno con daño 10 y target Solo Enemigo
        var venenoItem = ScriptableObject.CreateInstance<ObjetoDanino>();
        venenoItem.valorDaño = 10;
        venenoItem.estiloSeleccionObjetivo = ObjetoConsumible.SOLOENEMIGO;

        // Usamos el veneno sobre el enemigo
        venenoItem.Ejecutar(new List<Luchador> { enemigo });

        // Verificamos que ha hecho daño
        Assert.AreEqual(90, enemigo.vida, "El enemigo debería recibir daño del veneno.");

        // Limpieza
        Object.DestroyImmediate(enemigoGO);
    }

    [Test]
    public void PocionLimpiezaCuraSangrado()
    {
        // Aplicamos el estado 'Sangrado' al jugador
        jugador.AplicarEstado(EstadoAlterado.Sangrado);
        
        // Creamos la poción de limpieza para target Solo Jugador/Aliado (o Self)
        var pocionLimpieza = ScriptableObject.CreateInstance<ObjetoCuraEstado>();
        pocionLimpieza.estadoACurar = EstadoAlterado.Sangrado;
        pocionLimpieza.estiloSeleccionObjetivo = ObjetoConsumible.SOLOJUGADOR;

        // Usamos la poción
        pocionLimpieza.Ejecutar(new List<Luchador> { jugador });

        // Verificamos que el jugador ya NO tiene sangrado
        Assert.IsFalse(jugador.TieneEstado(EstadoAlterado.Sangrado), "El jugador debería haberse curado del Sangrado.");
    }

    [Test]
    public void InyeccionLucidezCuraAturdimiento()
    {
        // Aplicamos el estado 'Aturdimiento' al jugador
        jugador.AplicarEstado(EstadoAlterado.Aturdimiento);
        
        // Creamos la inyección para target Solo Jugador
        var inyeccion = ScriptableObject.CreateInstance<ObjetoCuraEstado>();
        inyeccion.estadoACurar = EstadoAlterado.Aturdimiento;
        inyeccion.estiloSeleccionObjetivo = ObjetoConsumible.SOLOJUGADOR;

        // Usamos el objeto
        inyeccion.Ejecutar(new List<Luchador> { jugador });

        // Verificamos que el estado ha desaparecido
        Assert.IsFalse(jugador.TieneEstado(EstadoAlterado.Aturdimiento), "El jugador debería haberse curado del Aturdimiento.");
    }

    [Test]
    public void SerumAntiToxinasCuraVeneno()
    {
        // Aplicamos el estado 'Veneno' al jugador
        jugador.AplicarEstado(EstadoAlterado.Veneno);
        
        // Creamos el serum para target Solo Jugador
        var serum = ScriptableObject.CreateInstance<ObjetoCuraEstado>();
        serum.estadoACurar = EstadoAlterado.Veneno;
        serum.estiloSeleccionObjetivo = ObjetoConsumible.SOLOJUGADOR;

        // Usamos el serum
        serum.Ejecutar(new List<Luchador> { jugador });

        // Verificamos que el estado Veneno ha sido eliminado
        Assert.IsFalse(jugador.TieneEstado(EstadoAlterado.Veneno), "El jugador debería haberse curado del Veneno.");
    }
}
