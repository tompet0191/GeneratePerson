using System;
using System.Text;

namespace GeneratePerson
{
    class Program
    {
        private static void Main(string[] args)
        {
            var p = new Person();

            Console.WriteLine(p.ToFormattedJson());

            Console.WriteLine(p.ToXml());

            
            #if DEBUG
                Console.WriteLine();
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            #endif
        }
    }

    
}
