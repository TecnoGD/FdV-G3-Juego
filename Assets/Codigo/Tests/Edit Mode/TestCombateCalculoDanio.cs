using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Codigo.Scripts;

public class TestCombateCalculoDanio
{
    private GameObject globalGO;
    private GameObject jugadorGO;
    private Luchador jugador;
    private GameObject enemigoGO;
    private Luchador enemigo;

    [SetUp]
    public void SetUp()
    {
        // Crear GLOBAL
        globalGO = new GameObject("GLOBAL");
        globalGO.AddComponent<GLOBAL>();

        // Crear acciones mock: Ataque físico (índice 0) y Ataque especial (índice 1)
        Ataque ataqueFisico = ScriptableObject.CreateInstance<Ataque>();
        ataqueFisico.Nombre = "Ataque";
        ataqueFisico.DañoBase = 20;
        ataqueFisico.tipo = Ataque.FISICO;
        ataqueFisico.EstiloSeleccionObjetivo = Accion.MONOOBJETIVO;

        Ataque ataqueEspecial = ScriptableObject.CreateInstance<Ataque>();
        ataqueEspecial.Nombre = "Ataque Esp";
        ataqueEspecial.DañoBase = 30;
        ataqueEspecial.tipo = Ataque.ESPECIAL;
        ataqueEspecial.EstiloSeleccionObjetivo = Accion.MONOOBJETIVO;

        // Inicializar acciones globales: índice 0 = Ataque físico, índice 1 = Ataque especial
        GLOBAL.acciones = new List<Accion> { ataqueFisico, ataqueEspecial };

        // Crear jugador
        jugadorGO = new GameObject("Jugador");
        jugadorGO.tag = "Luchador Jugador";
        jugador = jugadorGO.AddComponent<Luchador>();
        jugadorGO.AddComponent<Animator>();
        jugador.datos = ScriptableObject.CreateInstance<DatosLuchador>();
        jugador.datos.acciones = new List<int> { 0, 1 };
        jugador.datos.VidaMax = 100;
        jugador.datos.Ataque = 10;
        jugador.datos.Defensa = 5;
        jugador.datos.AtaqueEspecial = 15;
        jugador.datos.DefensaEspecial = 8;
        jugador.InicioCombate();

        // Crear enemigo
        enemigoGO = new GameObject("Enemigo");
        enemigoGO.tag = "Luchador Enemigo";
        enemigo = enemigoGO.AddComponent<Luchador>();
        enemigoGO.AddComponent<Animator>();
        enemigo.datos = ScriptableObject.CreateInstance<DatosLuchador>();
        enemigo.datos.acciones = new List<int> { 0 };
        enemigo.datos.VidaMax = 50;
        enemigo.datos.Ataque = 8;
        enemigo.datos.Defensa = 3;
        enemigo.datos.AtaqueEspecial = 12;
        enemigo.datos.DefensaEspecial = 5;
        enemigo.InicioCombate();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(jugadorGO);
        Object.DestroyImmediate(enemigoGO);
        Object.DestroyImmediate(globalGO);
        GLOBAL.acciones?.Clear();
    }

    [Test]
    public void TestCalculoDanioFisico()
    {
        // establece las estadisticas del jugador y enemigo para el test
        jugador.datos.Ataque = 20;
        jugador.estadisticas.ataque = 20;
        jugador.accion = 0;
        jugador.objetivosSeleccionados.Clear();
        jugador.objetivosSeleccionados.Add(enemigo);

        enemigo.datos.Defensa = 5;
        enemigo.estadisticas.defensa = 5;
        enemigo.vida = 100;
        enemigo.defiende = false;
        // obtiene la vida inicial del enemigo
        int vidaInicial = enemigo.vida;
        jugador.ProducirDaño();
        // calcula el daño minimo y maximo
        int danioMin = (int)(0.9f * 20f * (100f / (100f + 5f)));
        int danioMax = (int)(1.1f * 20f * (100f / (100f + 5f)));

        // verifica que el daño este entre el minimo y maximo
        Assert.IsTrue(
            enemigo.vida <= vidaInicial - danioMin &&
            enemigo.vida >= vidaInicial - danioMax,
            $"El daño físico debe estar entre {danioMin} y {danioMax}, vida final: {enemigo.vida}");
    }

    [Test]
    public void TestCalculoDanioEspecial()
    {
        // establece las estadisticas del jugador y enemigo para el test
        jugador.datos.AtaqueEspecial = 30;
        jugador.estadisticas.ataqueEspecial = 30;
        jugador.accion = 1;
        jugador.objetivosSeleccionados.Clear();
        jugador.objetivosSeleccionados.Add(enemigo);

        enemigo.datos.DefensaEspecial = 10;
        enemigo.estadisticas.defensaEspecial = 10;
        enemigo.vida = 100;
        enemigo.defiende = false;
        // obtiene la vida inicial del enemigo
        int vidaInicial = enemigo.vida;
        jugador.ProducirDaño();
        // calcula el daño minimo y maximo
        int danioMin = (int)(0.9f * 30f * (100f / (100f + 10f)));
        int danioMax = (int)(1.1f * 30f * (100f / (100f + 10f)));
        // verifica que el daño este entre el minimo y maximo
        Assert.IsTrue(
            enemigo.vida <= vidaInicial - danioMin &&
            enemigo.vida >= vidaInicial - danioMax,
            $"El daño especial debe estar entre {danioMin} y {danioMax}, vida final: {enemigo.vida}");
    }

    [Test]
    public void TestCalculoDanioConDefensa()
    {
        // establece las estadisticas del jugador y enemigo para el test
        jugador.datos.Ataque = 20;
        jugador.estadisticas.ataque = 20;
        jugador.accion = 0;
        jugador.objetivosSeleccionados.Clear();
        jugador.objetivosSeleccionados.Add(enemigo);

        enemigo.datos.Defensa = 5;
        enemigo.estadisticas.defensa = 5;
        enemigo.vida = 100;
        enemigo.defiende = true;
        // obtiene la vida inicial del enemigo
        int vidaInicial = enemigo.vida;
        jugador.ProducirDaño();
        // calcula el daño minimo y maximo
        int danioMin = (int)(0.9f * (20f * (100f / (100f + 5f))) / 2f);
        int danioMax = (int)(1.1f * (20f * (100f / (100f + 5f))) / 2f);
        // verifica que el daño este entre el minimo y maximo
        Assert.IsTrue(
            enemigo.vida <= vidaInicial - danioMin &&
            enemigo.vida >= vidaInicial - danioMax,
            $"El daño con defensa debe estar entre {danioMin} y {danioMax}, vida final: {enemigo.vida}");
    }

    [Test]
    public void TestCalculoDanioMinimo()
    {
        // establece las estadisticas del jugador y enemigo para el test
        jugador.datos.Ataque = 1;
        jugador.estadisticas.ataque = 1;
        jugador.accion = 0;
        jugador.objetivosSeleccionados.Clear();
        jugador.objetivosSeleccionados.Add(enemigo);

        enemigo.datos.Defensa = 100;
        enemigo.estadisticas.defensa = 100;
        enemigo.vida = 100;
        enemigo.defiende = false;
        // obtiene la vida inicial del enemigo
        int vidaInicial = enemigo.vida;
        jugador.ProducirDaño();
        // Siempre debe aplicar un daño mínimo de 1, independientemente del random.
        Assert.AreEqual(vidaInicial - 1, enemigo.vida, "El daño mínimo debe ser siempre 1");
    }

    [Test]
    public void TestCalculoDanioExcedeVida()
    {
        // establece las estadisticas del jugador y enemigo para el test
        jugador.datos.Ataque = 100;
        jugador.estadisticas.ataque = 100;
        jugador.accion = 0;
        jugador.objetivosSeleccionados.Clear();
        jugador.objetivosSeleccionados.Add(enemigo);

        enemigo.datos.Defensa = 1;
        enemigo.estadisticas.defensa = 1;
        enemigo.vida = 10;
        enemigo.defiende = false;

        jugador.ProducirDaño();

        Assert.AreEqual(0, enemigo.vida, "La vida debe quedar a 0 si el daño excede la vida y nunca menor");
    }
}

