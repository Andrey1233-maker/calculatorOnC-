using System;
using System.Runtime.CompilerServices;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Info();
            string f = Console.ReadLine();
            Formula F = new HardFormula(f);
            Console.WriteLine(F.getResult());
        }

        static void Info()
        {
            Console.WriteLine("Калькулятор на С#");
            Console.WriteLine("Операции, на которые способен калькулятор:");
            Console.WriteLine("+/- - сложение/вычитание");
            Console.WriteLine("*// - умножение/делние");
            Console.WriteLine("^ - возведние в степень");
            Console.WriteLine("() - для изменения очереди операций");
            Console.WriteLine("pi - число пи");
            Console.WriteLine("e - экпонента");
            Console.WriteLine();
        }
    }
}
