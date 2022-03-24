using System;

namespace Test.CopyPartialGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = new MyClaseA();
            var b = new MyClaseB();
            var c = new MyClaseC();
            var d = new MyClaseD();
            Console.WriteLine(a.Name);
            Console.WriteLine(a.Saludo);
            Console.WriteLine(a.Despedida());
            Console.WriteLine(b.Name);
            Console.WriteLine(b.Saludo);
            Console.WriteLine(b.Despedida());
            Console.WriteLine(c.Name);
            Console.WriteLine(c.Saludo);
            Console.WriteLine(c.Despedida());
            Console.WriteLine(d.Name);
            Console.WriteLine(d.Saludo);
            Console.WriteLine(d.Despedida());
        }
    }
}
