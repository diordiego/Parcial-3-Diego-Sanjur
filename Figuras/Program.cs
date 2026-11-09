// Parte 1 - Jerarquía de clases con clase abstracta y polimorfismo

// 1. Clase abstracta: molde obligatorio para todas las figuras
public abstract class Figura
{
    public abstract double CalcularArea();
    public abstract double CalcularPerimetro();
}

// 2. Clases derivadas que implementan (override) los métodos abstractos
public class Rectangulo : Figura
{
    public double Base { get; set; }
    public double Altura { get; set; }

    public Rectangulo(double baseRect, double altura)
    {
        Base = baseRect;
        Altura = altura;
    }

    public override double CalcularArea() => Base * Altura;
    public override double CalcularPerimetro() => 2 * (Base + Altura);
}

public class Cuadrado : Figura
{
    public double Lado { get; set; }

    public Cuadrado(double lado)
    {
        Lado = lado;
    }

    public override double CalcularArea() => Lado * Lado;
    public override double CalcularPerimetro() => 4 * Lado;
}

public class Triangulo : Figura
{
    public double LadoA { get; set; }
    public double LadoB { get; set; }
    public double LadoC { get; set; }

    public Triangulo(double ladoA, double ladoB, double ladoC)
    {
        LadoA = ladoA;
        LadoB = ladoB;
        LadoC = ladoC;
    }

    // Área con la fórmula de Herón
    public override double CalcularArea()
    {
        double s = CalcularPerimetro() / 2;
        return Math.Sqrt(s * (s - LadoA) * (s - LadoB) * (s - LadoC));
    }

    public override double CalcularPerimetro() => LadoA + LadoB + LadoC;
}

// Clase principal
public class Program
{
    public static void Main()
    {
        // 3. Arreglo unidimensional (vector) de tipo padre Figura
        Figura[] figuras = new Figura[3];
        figuras[0] = new Rectangulo(5, 3);
        figuras[1] = new Cuadrado(4);
        figuras[2] = new Triangulo(3, 4, 5);

        Console.WriteLine("Figuras geométricas\n");

        // 4. Recorrido polimórfico 
        foreach (Figura figura in figuras)
        {
            Console.WriteLine($"Figura:    {figura.GetType().Name}");
            Console.WriteLine($"Área:      {figura.CalcularArea():F2}");
            Console.WriteLine($"Perímetro: {figura.CalcularPerimetro():F2}");
            Console.WriteLine(new string('-', 30));
        }
    }
}