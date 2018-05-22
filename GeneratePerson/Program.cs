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
            var p = new Person(null, false);

            #if DEBUG
            //Test over 18
            var birthdates = new List<DateTime>();

            for (int x = 0; x < 1000; ++x)
            {
                var y = new Person(null, true);
                birthdates.Add(y.BirthDate);
            }

            foreach (var bd in birthdates)
            {
                int age = DateTime.Now.Year - bd.Year;
                if ((DateTime.Now.DayOfYear < bd.DayOfYear) && age <= 18)
                    age--;
                Debug.Assert(age >= 18, "Generated birthdate is under 18: " + bd.ToString());
            }
            //END
            #endif

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
