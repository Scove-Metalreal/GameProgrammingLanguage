namespace Day1;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(sum());
        Console.WriteLine(sub());
        Console.WriteLine(product());
        Console.WriteLine(divide());
       
    }


    static double sum()
    {
        double p1, p2;
        Console.WriteLine("Enter first number: ");
        p1 = double.Parse(Console.ReadLine());
        Console.WriteLine("Enter second number: ");
        p2 = double.Parse(Console.ReadLine());
        return p1 + p2;
    }

    static double sub()
    {
        double p1, p2;
        Console.WriteLine("Enter first number: ");
        p1 = double.Parse(Console.ReadLine());
        Console.WriteLine("Enter second number: ");
        p2 = double.Parse(Console.ReadLine());
        return p1 - p2;
    }

    static double product()
    {
        double p1, p2;
        Console.WriteLine("Enter first number: ");
        p1 = double.Parse(Console.ReadLine());
        Console.WriteLine("Enter second number: ");
        p2 = double.Parse(Console.ReadLine());
        return p1 * p2;
    }

    static double divide()
    {
        double p1, p2;
        Console.WriteLine("Enter first number: ");
        p1 = double.Parse(Console.ReadLine());
        Console.WriteLine("Enter second number: ");
        p2 = double.Parse(Console.ReadLine());
        try
        {
            return p1 / p2;
        }
        catch (DivideByZeroException)
        {
            return 0;
        }
    }
}