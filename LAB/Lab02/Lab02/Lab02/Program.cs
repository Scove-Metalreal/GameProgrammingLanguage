using System.Threading.Channels;

namespace Lab02;

class Program
{
    static void Main(string[] args)
    {
        int x = 0 , y = 0;
        string filePath = "input.txt";
        StreamWriter writer = null;
        try
        {
            Console.WriteLine("Enter the first number: ");
            x = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the second number: ");
            y = Convert.ToInt32(Console.ReadLine());

            writer = new StreamWriter(filePath);

            try
            {
                int result = Calculate(x, y);
                writer.WriteLine("Calculation result: " + result);
                Console.WriteLine("Result written to file.");
            }
            catch (NotNegativeException e)
            {
                writer.WriteLine("Calculation error: " + e.Message);
                Console.WriteLine("Result written to file.");
            }
            catch (DivideByZeroException e)
            {
                writer.WriteLine("Division error: " + e.Message);
                Console.WriteLine("Result written to file.");
            }
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (IOException e)
        {
            Console.WriteLine("File error: " + e.Message);
        }
        finally
        {
            if (writer != null)
            {
                writer.Close();
                Console.WriteLine("Closing the file.");
            }
        }

    }

    static int Calculate(int x, int y)
    {
        if ((Math.Sqrt(3 * x + 2 * y)) <= 0)
        {
            throw new NotNegativeException("Square root of 3 x 2 x 3 is negative");
        }

        if ((2 * x - y) == 0)
        {
            throw new DivideByZeroException("Divide by zero is invalid");
        }
        
        return (int)(Math.Sqrt(3 * x + 2 * y)) / (2 * x - y);
        
    }

    public class NotNegativeException : Exception
    {
        public NotNegativeException(string message) : base(message) { }
    }
}