using System;
using System.Collections.Generic;

//Ejercicio 1: Clase base Vehiculo (abstracción, encapsulación y validación)
abstract class Vehiculo
{
    public string Matricula { get; set; }

    private double _consumoCombustible;

    // Propiedad con validación (litros/100km)
    public double ConsumoCombustible
    {
        get { return _consumoCombustible; }
        set
        {
            if (value >= 0)
            {
                _consumoCombustible = value;
            }
            else
            {
                _consumoCombustible = 0.0;
            }
        }
    }

    // Propiedad de solo lectura
    public double CostoOperacionalBase
    {
        get { return 0.15; }
    }

    // Constructor base
    public Vehiculo(string matricula, double consumoCombustible)
    {
        Matricula = matricula;
        ConsumoCombustible = consumoCombustible;
    }

    // Método polimórfico abstracto
    public abstract double CalcularCostoPorKm();

    // ToString virtual
    public override string ToString()
    {
        return $"Matrícula: {Matricula} | Consumo: {ConsumoCombustible:F2} L/100km | Costo base: {CostoOperacionalBase:C2}/L";
    }
}

// Ejercicio 2: Vehículo de pasajeros (Autobús)
class Autobus : Vehiculo
{
    private double _capacidadMaxima;

    public double CapacidadMaxima
    {
        get { return _capacidadMaxima; }
        set
        {
            if (value >= 0)
            {
                _capacidadMaxima = value;
            }
            else
            {
                _capacidadMaxima = 0.0;
            }
        }
    }

    public double FactorDesgaste
    {
        get { return 1.2; }
    }

    // Constructor
    public Autobus(string matricula, double consumoCombustible, double capacidadMaxima)
        : base(matricula, consumoCombustible)
    {
        CapacidadMaxima = capacidadMaxima;
    }

    // Implementación polimórfica
    public override double CalcularCostoPorKm()
    {
        double costo = ConsumoCombustible * CostoOperacionalBase * FactorDesgaste;
        return costo;
    }

    public override string ToString()
    {
        return $"[Autobús] {base.ToString()} | Capacidad: {CapacidadMaxima} pasajeros | Factor desgaste: {FactorDesgaste:F2} | Costo por km: {CalcularCostoPorKm():C2}";
    }
}

// Ejercicio 3: Vehículo de carga (Camión)
class Camion : Vehiculo
{
    private double _peajeAnual;

    public double PeajeAnual
    {
        get { return _peajeAnual; }
        set
        {
            if (value >= 0)
            {
                _peajeAnual = value;
            }
            else
            {
                _peajeAnual = 0.0;
            }
        }
    }

    // Constructor
    public Camion(string matricula, double consumoCombustible, double peajeAnual)
        : base(matricula, consumoCombustible)
    {
        PeajeAnual = peajeAnual;
    }

    // Implementación polimórfica
    public override double CalcularCostoPorKm()
    {
        double costoBase = ConsumoCombustible * CostoOperacionalBase;
        double costoPeaje = PeajeAnual / 100000.0;
        return costoBase + costoPeaje;
    }

    public override string ToString()
    {
        return $"[Camión] {base.ToString()} | Peaje anual: {PeajeAnual:C2} | Costo por km: {CalcularCostoPorKm():C2}";
    }
}

// Ejercicio 4: Integración del Sistema de Consola (Polimorfismo con Colecciones)
class Program
{
    static List<Vehiculo> flota = new List<Vehiculo>();

    static void Main()
    {
        int opcion;
        do
        {
            MostrarMenu();
            opcion = LeerOpcion();

            switch (opcion)
            {
                case 1:
                    RegistrarVehiculo();
                    break;
                case 2:
                    VerCostosOperacionales();
                    break;
                case 3:
                    CalcularCostoTotalFlota();
                    break;
                case 4:
                    Console.WriteLine("Saliendo del sistema...");
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

        } while (opcion != 4);
    }

    static void MostrarMenu()
    {
        Console.WriteLine("\n========== FleetManager - Sistema de Gestión de Vehículos ==========");
        Console.WriteLine("1. Registrar Vehículo");
        Console.WriteLine("2. Ver Costos Operacionales");
        Console.WriteLine("3. Calcular Costo Total de Flota (100,000 km por vehículo)");
        Console.WriteLine("4. Salir");
    }

    static int LeerOpcion()
    {
        while (true)
        {
            Console.Write("Seleccione una opción (1-4): ");
            int numero;
            bool esNumero = int.TryParse(Console.ReadLine(), out numero);
            if (esNumero && numero >= 1 && numero <= 4)
            {
                return numero;
            }
            Console.WriteLine("Entrada inválida. Intente nuevamente.");
        }
    }

    static void RegistrarVehiculo()
    {
        Console.WriteLine("\nSeleccione el tipo de vehículo:");
        Console.WriteLine("1. Autobús");
        Console.WriteLine("2. Camión");

        int tipo = 0;
        while (true)
        {
            Console.Write("Opción (1-2): ");
            bool esNumero = int.TryParse(Console.ReadLine(), out tipo);
            if (esNumero && tipo >= 1 && tipo <= 2)
            {
                break;
            }
            Console.WriteLine("Entrada inválida. Intente nuevamente.");
        }

        Console.Write("Matrícula del vehículo: ");
        string matricula = Console.ReadLine();

        double consumo = LeerDoublePositivo("Consumo (L/100km): ");

        if (tipo == 1)
        {
            double capacidad = LeerDoublePositivo("Capacidad máxima (pasajeros): ");
            flota.Add(new Autobus(matricula, consumo, capacidad));
            Console.WriteLine("Autobús registrado correctamente.");
        }
        else if (tipo == 2)
        {
            double peaje = LeerDoublePositivo("Peaje anual (€): ");
            flota.Add(new Camion(matricula, consumo, peaje));
            Console.WriteLine("Camión registrado correctamente.");
        }
    }

    static double LeerDoublePositivo(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            double valor;
            bool esNumero = double.TryParse(Console.ReadLine(), out valor);
            if (esNumero && valor >= 0)
            {
                return valor;
            }
            Console.WriteLine("Valor inválido. Debe ser un número no negativo.");
        }
    }

    static void VerCostosOperacionales()
    {
        Console.WriteLine("\n--- Costos Operacionales por Vehículo ---");
        if (flota.Count == 0)
        {
            Console.WriteLine("No hay vehículos registrados.");
            return;
        }

        int i = 1;
        foreach (Vehiculo v in flota)
        {
            Console.WriteLine($"{i}. {v}");
            i++;
        }
    }

    static void CalcularCostoTotalFlota()
    {
        if (flota.Count == 0)
        {
            Console.WriteLine("No hay vehículos registrados.");
            return;
        }

        double total = 0;
        double distancia = 100000.0; // km por vehículo

        foreach (Vehiculo v in flota)
        {
            total += v.CalcularCostoPorKm() * distancia;
        }

        Console.WriteLine($"\nCosto total estimado de la flota (100,000 km por vehículo): {total:C2}");
    }
}
