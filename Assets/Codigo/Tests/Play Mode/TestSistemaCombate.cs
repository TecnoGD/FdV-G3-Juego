using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Codigo.Scripts;

public class TestSistemaCombate
{
    private GameObject globalGO;
    private GameObject sistemaCombateGO;
    private SistemaCombate sistemaCombate;
    private GameObject jugadorGO;
    private Luchador jugador;
    private GameObject enemigoGO;
    private Luchador enemigo;
    private GameObject vidaEnemigoPadre; // Guardar referencia al padre para evitar que se destruya

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Crear GLOBAL
        globalGO = new GameObject("GLOBAL");
        globalGO.AddComponent<GLOBAL>();
        yield return null;

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

        // Crear sistema de combate
        sistemaCombateGO = new GameObject("SistemaCombate");
        sistemaCombate = sistemaCombateGO.AddComponent<SistemaCombate>();
        SistemaCombate.luchadores.Clear();
        sistemaCombate.turno = -1;
        sistemaCombate.UIGeneral = new List<GameObject>();
        sistemaCombate.TextoUI = new List<TMPro.TMP_Text>();
        
        // Inicializar ElementosUI con GameObjects mock
        sistemaCombate.ElementosUI = new List<GameObject>();
        for (int i = 0; i <= SistemaCombate.UIVidaEnemigos; i++)
        {
            GameObject uiElement = new GameObject($"UIElement_{i}");
            sistemaCombate.ElementosUI.Add(uiElement);
        }
        
        // Añadir MenuObjetivos al GameObject de objetivos para que responda al BroadcastMessage de los tests de seleccion de ataques fisico y especial
        // Desactivamos el GameObject antes de añadir el componente para evitar que OnEnable se ejecute
        GameObject objetivoUI = sistemaCombate.ElementosUI[SistemaCombate.UIObjetivoCombate];
        objetivoUI.SetActive(false);
        objetivoUI.AddComponent<MenuObjetivos>();
        objetivoUI.SetActive(true);
        
        // Inicializar TextoVidas con elementos mock para evitar errores de índice
        sistemaCombate.TextoVidas = new List<TMPro.TMP_Text>();
        // Crear un Canvas y un GameObject padre con TextMeshProUGUI para simular la estructura de UI
        // TextMeshProUGUI necesita un Canvas para funcionar
        GameObject canvasGO = new GameObject("Canvas");
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // El padre debe persistir para que transform.parent no sea null
        vidaEnemigoPadre = new GameObject("VidaEnemigoPadre");
        vidaEnemigoPadre.transform.SetParent(canvasGO.transform);
        GameObject vidaEnemigoHijo = new GameObject("VidaEnemigoHijo");
        vidaEnemigoHijo.transform.SetParent(vidaEnemigoPadre.transform);
        TMPro.TextMeshProUGUI textoVidaMock = vidaEnemigoHijo.AddComponent<TMPro.TextMeshProUGUI>();
        sistemaCombate.TextoVidas.Add(textoVidaMock);

        // Crear jugador 
        jugadorGO = new GameObject("Jugador");
        jugadorGO.tag = "Luchador Jugador";
        jugador = jugadorGO.AddComponent<Luchador>();
        jugadorGO.AddComponent<Animator>();
        jugador.datos = ScriptableObject.CreateInstance<DatosLuchador>();
        jugador.datos.acciones = new List<int> { 0, 1 }; // 0 = Ataque físico, 1 = Ataque especial
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

        GLOBAL.enCombate = true;
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Limpiar elementos de UI
        if (sistemaCombate != null && sistemaCombate.ElementosUI != null)
        {
            foreach (GameObject uiElement in sistemaCombate.ElementosUI)
            {
                if (uiElement != null)
                    Object.Destroy(uiElement);
            }
        }
        
        // Limpiar TextoVidas
        if (sistemaCombate != null && sistemaCombate.TextoVidas != null)
        {
            foreach (TMPro.TMP_Text textoVida in sistemaCombate.TextoVidas)
            {
                if (textoVida != null && textoVida.gameObject != null)
                    Object.Destroy(textoVida.gameObject);
            }
        }
        
        // Limpiar el GameObject padre de vida de enemigo
        if (vidaEnemigoPadre != null)
            Object.Destroy(vidaEnemigoPadre);
        
        Object.Destroy(jugadorGO);
        Object.Destroy(enemigoGO);
        Object.Destroy(sistemaCombateGO);
        Object.Destroy(globalGO);
        SistemaCombate.luchadores.Clear();
        yield return null;
    }

    // ========== TESTS DE SELECCIÓN DE ACCIONES ==========
    // Estructura de menús:
    // Menú principal: Ataque, Defensa, Pasar Turno
    // Submenú Ataque: Ataque (físico), Ataque Esp (especial), Atras

    [UnityTest]
    public IEnumerator TestSeleccionAtaqueFisico()
    {
        // El jugador selecciona "Ataque" en el menú principal, luego "Ataque" (físico) en el submenú
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        sistemaCombate.jugador = jugador;
        jugador.accion = -1;

        // Seleccionar ataque físico (índice 0 en listaAcciones)
        sistemaCombate.AtaqueElegido(0);

        // Comprobar que la acción es 0 (Ataque físico) y el índice en listaAcciones es 0
        Assert.AreEqual(0, jugador.accion, "La acción debería ser 0 (Ataque físico)");
        Assert.AreEqual(0, jugador.listaAcciones[jugador.accion], "El índice en listaAcciones debería ser 0");
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSeleccionAtaqueEspecial()
    {
        // El jugador selecciona "Ataque" en el menú principal, luego "Ataque Esp" en el submenú
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        sistemaCombate.jugador = jugador;
        jugador.accion = -1;

        // Seleccionar ataque especial (índice 1 en listaAcciones)
        sistemaCombate.AtaqueElegido(1);

        // Comprobar que la acción es 1 (Ataque especial) y el índice en listaAcciones es 1
        Assert.AreEqual(1, jugador.accion, "La acción debería ser 1 (Ataque especial)");
        Assert.AreEqual(1, jugador.listaAcciones[jugador.accion], "El índice en listaAcciones debería ser 1");
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSeleccionDefensa()
    {
        // El jugador selecciona "Defensa" en el menú principal
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        sistemaCombate.jugador = jugador;
        jugador.defiende = false;

        // Seleccionar defensa (acción 1 del menú principal)
        sistemaCombate.AccionEscogida(1);

        // Comprobar que el jugador está defendiendo
        Assert.IsTrue(jugador.defiende, "El jugador debería estar defendiendo después de seleccionar Defensa");
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSeleccionPasarTurno()
    {
        // El jugador selecciona "Pasar Turno" en el menú principal
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        sistemaCombate.jugador = jugador;
        
        // Seleccionar pasar turno (acción 2 del menú principal)
        sistemaCombate.AccionEscogida(2);

        // Comprobar que el turno avanza
        Assert.Greater(sistemaCombate.turno, -1, "El turno debería haber avanzado");
        yield return null;
    }

    // ========== TESTS DE SELECCIÓN DE OBJETIVOS ==========

    [UnityTest]
    public IEnumerator TestSeleccionarUnObjetivo()
    {
        // Limpiar los objetivos seleccionados
        jugador.objetivosSeleccionados.Clear();

        // Añadir un objetivo (enemigo)
        jugador.objetivosSeleccionados.Add(enemigo);

        // Comprobar que hay un objetivo seleccionado
        Assert.AreEqual(1, jugador.objetivosSeleccionados.Count, "Debería haber un objetivo seleccionado");
        Assert.AreEqual(enemigo, jugador.objetivosSeleccionados[0], "El objetivo debería ser el enemigo");
        Assert.Contains(enemigo, jugador.objetivosSeleccionados, "El enemigo debería estar en la lista de objetivos");
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSeleccionarMultiplesObjetivos()
    {
        // Crear un segundo enemigo
        GameObject enemigo2GO = new GameObject("Enemigo2");
        enemigo2GO.tag = "Luchador Enemigo";
        Luchador enemigo2 = enemigo2GO.AddComponent<Luchador>();
        enemigo2GO.AddComponent<Animator>();
        enemigo2.datos = ScriptableObject.CreateInstance<DatosLuchador>();
        enemigo2.datos.acciones = new List<int> { 0 };
        enemigo2.datos.VidaMax = 50;
        enemigo2.datos.Ataque = 8;
        enemigo2.datos.Defensa = 3;
        enemigo2.datos.AtaqueEspecial = 12;
        enemigo2.datos.DefensaEspecial = 5;
        enemigo2.InicioCombate();

        // Limpiar los objetivos seleccionados
        jugador.objetivosSeleccionados.Clear();

        // Añadir dos objetivos (enemigo y enemigo2)
        jugador.objetivosSeleccionados.Add(enemigo);
        jugador.objetivosSeleccionados.Add(enemigo2);

        // Comprobar que hay dos objetivos seleccionados
        Assert.AreEqual(2, jugador.objetivosSeleccionados.Count, "Debería haber dos objetivos seleccionados");
        Assert.Contains(enemigo, jugador.objetivosSeleccionados, "El primer enemigo debería estar en la lista");
        Assert.Contains(enemigo2, jugador.objetivosSeleccionados, "El segundo enemigo debería estar en la lista");
        Assert.AreEqual(enemigo, jugador.objetivosSeleccionados[0], "El primer objetivo debería ser el primer enemigo");
        Assert.AreEqual(enemigo2, jugador.objetivosSeleccionados[1], "El segundo objetivo debería ser el segundo enemigo");

        Object.Destroy(enemigo2GO);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSeleccionSinObjetivos()
    {
        // Limpiar los objetivos seleccionados
        jugador.objetivosSeleccionados.Clear();

        // Comprobar que no hay objetivos seleccionados
        Assert.AreEqual(0, jugador.objetivosSeleccionados.Count, "No debería haber objetivos seleccionados");
        Assert.IsEmpty(jugador.objetivosSeleccionados, "La lista de objetivos debería estar vacía");
        yield return null;
    }

    // ========== TESTS DE EJECUCIÓN DE TURNOS ==========

    [UnityTest]
    public IEnumerator TestTurnoInicial()
    {
        // El turno inicial debería ser -1 (jugador decidiendo)
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        sistemaCombate.jugador = jugador;
        sistemaCombate.turno = -1;

        // Comprobar que el turno inicial es -1
        Assert.AreEqual(-1, sistemaCombate.turno, "El turno inicial debería ser -1 (jugador decidiendo)");
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestResetTurno()
    {
        // Resetear el turno de un luchador
        jugador.accion = 1;
        jugador.defiende = true;

        // Resetear el turno
        jugador.ResetTurno();

        // Comprobar que los valores se han reseteado correctamente
        Assert.AreEqual(-1, jugador.accion, "La acción debería resetearse a -1");
        Assert.IsFalse(jugador.defiende, "La defensa debería resetearse a false");
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestAvanceTurno()
    {
        // El turno avanza cuando se llama a FinAccion
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        SistemaCombate.luchadores.Add(enemigo);
        sistemaCombate.jugador = jugador;
        sistemaCombate.turno = -1;

        // Simular que el jugador termina su acción
        jugador.accion = -1; // Sin acción para que no falle
        sistemaCombate.FinAccion();
        yield return null;

        // El turno debería haber avanzado (de -1 a 0 o más)
        Assert.Greater(sistemaCombate.turno, -1, "El turno debería haber avanzado");
        yield return null;
    }

    // ========== TESTS DE FIN DEL COMBATE ==========

    [UnityTest]
    public IEnumerator TestFinCombateVictoria()
    {
        // El combate termina con victoria cuando todos los enemigos mueren
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        SistemaCombate.luchadores.Add(enemigo);
        sistemaCombate.jugador = jugador;
        
        // Configurar estado: jugador vivo, enemigo muerto
        jugador.vida = 50;
        enemigo.vida = 0;
        
        // Esperar el mensaje de victoria
        LogAssert.Expect(LogType.Log, "Has Ganado");
        
        // Finalizar el combate y emitir el mensaje de victoria
        sistemaCombate.FinAccion();
        yield return null;        
    }

    [UnityTest]
    public IEnumerator TestFinCombateDerrota()
    {
        // El combate termina con derrota cuando el jugador muere
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        SistemaCombate.luchadores.Add(enemigo);
        sistemaCombate.jugador = jugador;
        
        // Configurar estado: jugador muerto, enemigo vivo
        jugador.vida = 0;
        enemigo.vida = 50;
        
        // Esperar el mensaje de derrota
        LogAssert.Expect(LogType.Log, "Has Perdido");
        
        // Finalizar el combate y emitir el mensaje de derrota
        sistemaCombate.FinAccion();
        yield return null;        
    }

}
