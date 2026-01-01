/*using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Codigo.Scripts;
using System.Linq;

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
    private GameObject menuSystemGO; 

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Mock de MenuSystem necesario para GLOBAL.Start()
        menuSystemGO = new GameObject("MenuSystem");
        MenuSystem menuSystem = menuSystemGO.AddComponent<MenuSystem>();
        MenuSystem.instance = menuSystem; // Asignamos instancia manualmente
        menuSystem.menuJugador = new GameObject("MenuJugadorMock"); // Mock del menú de jugador

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
        Object.Destroy(menuSystemGO);
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
        
        // Seleccionar pasar turno (acción -1 de AccionEscogida)
        sistemaCombate.AccionEscogida(-1);

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


    // ========== TESTS DE ANALIZAR ENEMIGO ========== 

    [UnityTest]
    public IEnumerator TestAnalizarEnemigo()
    {
        // Configurar EventSystem para la interacción con la UI 
        GameObject eventSystemGO = new GameObject("EventSystem");
        eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();

        // Configurar MenuObjetivos con los mocks necesarios
        GameObject objetivoUI = sistemaCombate.ElementosUI[SistemaCombate.UIObjetivoCombate];
        MenuObjetivos menuObjetivos = objetivoUI.GetComponent<MenuObjetivos>();

        // Mock de Prefabs (Simulamos los elementos visuales que se instancian)
        GameObject prefabToggle = new GameObject("PrefabToggle");
        prefabToggle.AddComponent<UnityEngine.UI.Toggle>();
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(prefabToggle.transform);
        textGO.AddComponent<UnityEngine.UI.Text>();
        menuObjetivos.prefabBotonObjetivo = prefabToggle;

        GameObject prefabAtras = new GameObject("PrefabAtras");
        prefabAtras.AddComponent<UnityEngine.UI.Button>();
        menuObjetivos.prefabBotonAtras = prefabAtras;

        // Configurar MenuAnalizar (nueva clase separada)
        GameObject menuAnalizarGO = new GameObject("MenuAnalizar");
        MenuAnalizar menuAnalizar = menuAnalizarGO.AddComponent<MenuAnalizar>();

        // Mock del Panel de Análisis
        GameObject panelAnalisis = new GameObject("PanelAnalisis");
        panelAnalisis.SetActive(false);
        menuAnalizar.panelAnalisis = panelAnalisis;

        // Mock de los Textos de Análisis
        menuAnalizar.textoNombreAnalisis = new GameObject("TxtNombre").AddComponent<TMPro.TextMeshProUGUI>();
        menuAnalizar.textoVidaAnalisis = new GameObject("TxtVida").AddComponent<TMPro.TextMeshProUGUI>();
        menuAnalizar.textoAtaqueAnalisis = new GameObject("TxtAtaque").AddComponent<TMPro.TextMeshProUGUI>();
        menuAnalizar.textoDefensaAnalisis = new GameObject("TxtDefensa").AddComponent<TMPro.TextMeshProUGUI>();
        menuAnalizar.textoAtaqueEspecialAnalisis = new GameObject("TxtAtqEsp").AddComponent<TMPro.TextMeshProUGUI>();
        menuAnalizar.textoDefensaEspecialAnalisis = new GameObject("TxtDefEsp").AddComponent<TMPro.TextMeshProUGUI>();

        // Mock del Botón Volver
        GameObject btnVolver = new GameObject("BtnVolver");
        menuAnalizar.botonVolverAnalisis = btnVolver.AddComponent<UnityEngine.UI.Button>();

        // Preparar el estado del combate
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        SistemaCombate.luchadores.Add(enemigo);
        sistemaCombate.jugador = jugador;
        GLOBAL.enCombate = true;
        
        // Establecer nombre del enemigo para verificación
        enemigo.nombre = "EnemigoTest";

        // Forzar que OnEnable se ejecute desactivando y reactivando el UI
        objetivoUI.SetActive(false);
        yield return null;
        objetivoUI.SetActive(true);
        yield return null;

        // Ejecutar la acción de Analizar
        sistemaCombate.Analizar();
        yield return null;

        // Simular la selección del enemigo
        Assert.IsTrue(objetivoUI.activeSelf, "MenuObjetivos debería estar activo");

        // Buscar el toggle. Como solo tenemos 1 enemigo, debería ser el primer toggle instanciado.
        UnityEngine.UI.Toggle enemyToggle = objetivoUI.GetComponentInChildren<UnityEngine.UI.Toggle>();
        Assert.IsNotNull(enemyToggle, "El toggle del enemigo debería haber sido creado");

        // Simular la selección del enemigo y mostrar análisis
        menuAnalizar.MostrarAnalisisEnemigo(enemigo);
        panelAnalisis.SetActive(true); // Activar manualmente el panel en el test
        yield return null;

        // Verificar el Panel de Análisis
        Assert.AreEqual("EnemigoTest", menuAnalizar.textoNombreAnalisis.text);
        Assert.AreEqual("Vida: " + enemigo.vida, menuAnalizar.textoVidaAnalisis.text);
        Assert.AreEqual("Ataque: " + enemigo.estadisticas.ataque, menuAnalizar.textoAtaqueAnalisis.text);
        Assert.AreEqual("Defensa: " + enemigo.estadisticas.defensa, menuAnalizar.textoDefensaAnalisis.text);
        Assert.AreEqual("Atq. Esp: " + enemigo.estadisticas.ataqueEspecial, menuAnalizar.textoAtaqueEspecialAnalisis.text);
        Assert.AreEqual("Def. Esp: " + enemigo.estadisticas.defensaEspecial, menuAnalizar.textoDefensaEspecialAnalisis.text);

        // Limpieza de objetos creados para el test
        if (eventSystemGO != null)
            Object.Destroy(eventSystemGO);
        Object.Destroy(prefabToggle);
        Object.Destroy(prefabAtras);
        Object.Destroy(menuAnalizarGO);
        Object.Destroy(panelAnalisis);
        Object.Destroy(menuAnalizar.textoNombreAnalisis.gameObject);
        Object.Destroy(menuAnalizar.textoVidaAnalisis.gameObject);
        Object.Destroy(menuAnalizar.textoAtaqueAnalisis.gameObject);
        Object.Destroy(menuAnalizar.textoDefensaAnalisis.gameObject);
        Object.Destroy(menuAnalizar.textoAtaqueEspecialAnalisis.gameObject);
        Object.Destroy(menuAnalizar.textoDefensaEspecialAnalisis.gameObject);
        Object.Destroy(btnVolver.gameObject);
        
        yield return null;
    }


    // ========== TESTS DE SISTEMA DE OBJETOS ==========

    [UnityTest]
    public IEnumerator TestEjecucionAccionConObjeto()
    {
        // Configurar el combate
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        SistemaCombate.luchadores.Add(enemigo);
        sistemaCombate.jugador = jugador;
        GLOBAL.enCombate = true;

        // Crear un objeto consumible mock (usamos ObjetoCurativo como ejemplo)
        ObjetoCurativo objetoMock = ScriptableObject.CreateInstance<ObjetoCurativo>();
        objetoMock.nombre = "Objeto Test";
        objetoMock.estiloSeleccionObjetivo = ObjetoConsumible.SOLOJUGADOR;
        objetoMock.valorCurativo = 10;

        // Configurar el slot de objeto del jugador
        jugador.objetosConsumibles = new ObjectSlot[1];
        jugador.objetosConsumibles[0] = new ObjectSlot(objetoMock, 2);

        // Configurar la acción de usar objeto
        jugador.objetoSeleccionado = 0;
        jugador.accion = -2; // Acción de usar objeto (accion < -1)
        jugador.objetivosSeleccionados.Clear();
        jugador.objetivosSeleccionados.Add(jugador);

        // Guardar estado inicial
        int cantidadInicial = jugador.objetosConsumibles[0].cantidad;

        // Ejecutar la acción
        jugador.EjecutarAccion(SistemaCombate.luchadores);
        yield return null;

        // Se comprueba que la cantidad del objeto se reduce
        Assert.AreEqual(cantidadInicial - 1, jugador.objetosConsumibles[0].cantidad, 
            "La cantidad del objeto debería reducirse después de ejecutar la acción");

        // Cleanup
        Object.Destroy(objetoMock);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSeleccionObjetoCorrecto()
    {
        // Configurar el combate
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        sistemaCombate.jugador = jugador;
        GLOBAL.enCombate = true;

        // Crear dos objetos diferentes
        ObjetoCurativo objeto1 = ScriptableObject.CreateInstance<ObjetoCurativo>();
        objeto1.nombre = "Objeto 1";
        objeto1.valorCurativo = 10;

        ObjetoCurativo objeto2 = ScriptableObject.CreateInstance<ObjetoCurativo>();
        objeto2.nombre = "Objeto 2";
        objeto2.valorCurativo = 20;

        // Configurar slots de objetos
        jugador.objetosConsumibles = new ObjectSlot[2];
        jugador.objetosConsumibles[0] = new ObjectSlot(objeto1, 3);
        jugador.objetosConsumibles[1] = new ObjectSlot(objeto2, 2);

        // Seleccionar el segundo objeto (índice 1)
        jugador.objetoSeleccionado = 1;
        jugador.accion = -2;
        jugador.objetivosSeleccionados.Clear();
        jugador.objetivosSeleccionados.Add(jugador);

        // Ejecutar la acción
        jugador.EjecutarAccion(SistemaCombate.luchadores);
        yield return null;

        // Verificar que se usó el objeto correcto (el segundo)
        Assert.AreEqual(3, jugador.objetosConsumibles[0].cantidad, 
            "El primer objeto NO debería haberse usado");
        Assert.AreEqual(1, jugador.objetosConsumibles[1].cantidad, 
            "El segundo objeto SÍ debería haberse usado (cantidad reducida de 2 a 1)");

        // Cleanup
        Object.Destroy(objeto1);
        Object.Destroy(objeto2);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestEliminacionObjetoAgotado()
    {
        // Configurar el combate
        SistemaCombate.luchadores.Clear();
        SistemaCombate.luchadores.Add(jugador);
        sistemaCombate.jugador = jugador;
        GLOBAL.enCombate = true;

        // Crear un objeto con cantidad = 1 (última unidad)
        ObjetoCurativo objetoMock = ScriptableObject.CreateInstance<ObjetoCurativo>();
        objetoMock.nombre = "Última Unidad";
        objetoMock.estiloSeleccionObjetivo = ObjetoConsumible.SOLOJUGADOR;
        objetoMock.valorCurativo = 15;

        // Configurar el slot con cantidad = 1
        jugador.objetosConsumibles = new ObjectSlot[1];
        jugador.objetosConsumibles[0] = new ObjectSlot(objetoMock, 1);

        // Usar el objeto
        jugador.objetoSeleccionado = 0;
        jugador.accion = -2;
        jugador.objetivosSeleccionados.Clear();
        jugador.objetivosSeleccionados.Add(jugador);

        // Ejecutar la acción
        jugador.EjecutarAccion(SistemaCombate.luchadores);
        yield return null;

        // Verificar que el objeto se eliminó del slot
        Assert.IsNull(jugador.objetosConsumibles[0].objeto, 
            "El objeto debería ser null cuando se agota (cantidad llega a 0)");
        Assert.AreEqual(-1, jugador.objetosConsumibles[0].cantidad, 
            "La cantidad debería ser -1 cuando el objeto se elimina del slot");

        // Cleanup
        Object.Destroy(objetoMock);
        yield return null;
    }

}*/


