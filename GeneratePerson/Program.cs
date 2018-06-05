using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GeneratePerson
{
    class Program
    {
        private static void Main(string[] args)
        {
            
            var p = new Person(new Name(), new Address(), new SwedishSsnoCalculator());
            for (int i = 0; i < 10; i++)
            {
                p.GenerateRandomData(false);
                Console.WriteLine(p.ToFormattedJson());
                Console.WriteLine(p.ToXml());
             
            }

            //var x = 0;
            //for (int i = 0; i < 100; i++)
            //{
            //    p.GenerateGender();
            //    if (p.Gender == "Female")
            //        x++;
            //}

            //Console.WriteLine(x);
            //var r = new Random();

            //for (int i = 0; i < 100; i++)
            //{
            //    Console.WriteLine(r.Next(100)); 
            //}

            //Console.WriteLine(x);



            


                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
           
        }
    }

    
}
