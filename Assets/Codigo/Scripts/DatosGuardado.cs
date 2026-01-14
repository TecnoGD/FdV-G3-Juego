using System;
using System.Collections.Generic;
using UnityEngine;



namespace Codigo.Scripts
{
    [System.Serializable]
    public class DatosGuardado : ICloneable
    {
        public int id;                                          // Variable de prueba
        public string nombre;                                   // Nombre del jugador
        public int vida;
        public DatosCombate.Estadisticas estadisticasJugador;   // Estadisticas base del jugador
        [SerializeField] public List<int> accionesJugador;                           // Lista de acciones del jugador
        [SerializeField] public List<DatosObjetoGuardado> objetosConsumibles;
        [SerializeField] public List<int> listasDeEquipamientosArmas;
        [SerializeField] public List<int> listasDeEquipamientosArmaduras;
        [SerializeField] public List<int> listasDeEquipamientosZapatos;
        [SerializeField] public List<int> listasDeEquipamientosAccesorios;
        public int[] equipamientoJugador;
        [NonSerialized] public List<ObjectSlot> ObjetosCargados;
        public int[] objetosSeleccionadosCombate;

        public int progresoHistoria; // Controla el avance de la historia (dialogo).
        public int actoActual = 1;
        public int combatesGanados = 0;
        public List<string> flagsEventos = new List<string>();
        public string nombreJugador = "Héroe";
        [SerializeField] public DiccionarioSerializableStringInt memoriaNPCs = new DiccionarioSerializableStringInt();

        public DatosGuardado(ObjetoConsumible obj)
        {
            estadisticasJugador = new DatosCombate.Estadisticas(50, 50, 20, 10, 1);
            vida = estadisticasJugador.vidaMax;
            id = 5;
            accionesJugador = new List<int>(new int[] {0,1,2});
            nombre = "Jugador"; 
            objetosConsumibles = new List<DatosObjetoGuardado>();
            objetosConsumibles.Add(new DatosObjetoGuardado(0, 1));
            objetosSeleccionadosCombate = new int[] {-1,-1,-1,-1};
            listasDeEquipamientosArmas = new List<int> { 0 };
            listasDeEquipamientosArmaduras = new List<int> { 0 };
            listasDeEquipamientosZapatos = new List<int> { 0 };
            listasDeEquipamientosAccesorios = new List<int> { 0 };
            equipamientoJugador = new [] {-1,-1,-1,-1};
            progresoHistoria = 0; // Empezamos desde 0 (inicio)
            
        }

        public void CargarObjetos()
        {
            
        }

        public object Clone()
        {
            DatosGuardado copia = (DatosGuardado)MemberwiseClone();
            copia.accionesJugador =  new List<int>(accionesJugador);
            copia.objetosConsumibles =  new List<DatosObjetoGuardado>(objetosConsumibles);
            copia.listasDeEquipamientosArmas = new List<int>(listasDeEquipamientosArmas);
            copia.listasDeEquipamientosArmaduras = new List<int>(listasDeEquipamientosArmaduras);
            copia.listasDeEquipamientosZapatos = new List<int>(listasDeEquipamientosZapatos);
            copia.listasDeEquipamientosAccesorios  = new List<int>(listasDeEquipamientosAccesorios);
            copia.equipamientoJugador = (int[])equipamientoJugador.Clone();
            copia.objetosSeleccionadosCombate =  (int[])objetosSeleccionadosCombate.Clone();
            copia.flagsEventos = new List<string>(flagsEventos);
            copia.memoriaNPCs = new DiccionarioSerializableStringInt(memoriaNPCs);
            return copia;
        }
    }

    [Serializable]
    public class DatosObjetoGuardado
    {
        public int id;
        public int cantidad;

        public DatosObjetoGuardado(int id, int cantidad)
        {
            this.id = id;
            this.cantidad = cantidad;
        }

    }
}