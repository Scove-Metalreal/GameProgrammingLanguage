using System.Threading.Channels;

namespace Lab02;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        double a = int.Parse(Console.ReadLine());
        double b = int.Parse(Console.ReadLine());
        Console.WriteLine("Thuong la: " + divide(a, b));
    }

    static double divide(double a, double b)
    {
        try
        {
            return a / b;
        }
        catch (DivideByZeroException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Source);
            Console.WriteLine(e.StackTrace);
            Console.WriteLine(e.InnerException);
            throw new DivideByZeroException("Division by zero", e);
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.Message);
            throw new FormatException("Format", e);
        }
        catch (ArithmeticException e)
        {
            Console.WriteLine(e.Message);
            throw new ArithmeticException("Arithmetic", e);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Unknown Exception");
        }
        finally
        {
            Console.WriteLine("finally:");
        }
    }
}