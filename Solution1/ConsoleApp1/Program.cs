using System;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // var input = Console.ReadLine();
            // Console.WriteLine(Reverse(input));
            
            Console.WriteLine(IsPalindrom(Console.ReadLine()));
        }

        static bool IsPalindrom(string input)
        {
            // bqlhlqb
            for (int i = 0; i < input.Length; i++)
            {
                var j = input.Length - i - 1;
                if (input[i] != input[j])
                {
                    return false;
                }
            }

            return true;
        }
        
        static string Reverse(string input)
        {
            var result = new StringBuilder();
            for (int i = input.Length - 1; i >= 0; i--)
            {
                result.Append(input[i]);
            }

            return result.ToString();
        }

        static long Factorial(int n)
        {
            if (n <= 1)
            {
                return 1;
            }

            return n * (Factorial(n - 1));
        }
    }
}