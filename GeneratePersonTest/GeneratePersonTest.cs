using GeneratePerson;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GeneratePersonTest
{
    [TestFixture]
    public class GeneratePersonTest
    {

        public Person TestPerson { get; set; }

        [SetUp]
        public void GeneratePersonTestSetUp()
        {
            TestPerson = new Person(TestContext.CurrentContext.WorkDirectory);
        }

        [Test]
        public void ShouldGeneratePersonOver18()
        {
            var birthdates = new List<DateTime>();

            for (var x = 0; x < 100000; ++x)
            {
                TestPerson.GenerateBirthDate(true);

                birthdates.Add(TestPerson.BirthDate);
            }

            foreach (var bd in birthdates)
            {
                var age = DateTime.Now.Year - bd.Year;
                if ((DateTime.Now.DayOfYear < bd.DayOfYear) && age <= 18)
                    age--;

                Assert.That(age, Is.AtLeast(18));
            }
        }

        [Test]
        public void ShouldGenerateCorrectPin()
        {
            var pins = new List<string>();

            
            for (var x = 0; x < 100000; ++x)
            {
                TestPerson.GenerateGender();
                TestPerson.GenerateBirthDate(false);
                TestPerson.GenerateSocialSecurityNumber();

                pins.Add(TestPerson.SocialSecurityNumber);
            }

            foreach(var pin in pins)
            {
                Assert.That( FixPinFormat(pin), Is.True );
                Assert.That( CheckPIN(pin), Is.True );
            }
        }

        private static bool FixPinFormat(string str)
        {
            var r = new Regex("[^0-9]");
            var r3 = new Regex(@"^\d{10}$");

            str = r.Replace(str, "");

            return r3.IsMatch(str);
        }

        private static bool CheckPIN(string str)
        {
            var sum = 0;
            for (var x = 9; x >= 0; x--)
            {
                if (x % 2 == 1)
                    sum += (int)char.GetNumericValue(str[x]);
                else
                {

                    var y = (int)char.GetNumericValue(str[x]) * 2;
                    if (y > 9)
                    {
                        var list = y.ToString().Select(w => (int)char.GetNumericValue(w)).ToArray();

                        sum += list.Sum();
                    }
                    else
                        sum += (int)char.GetNumericValue(str[x]) * 2;
                }

            }
            return sum % 10 == 0;
        }

        [TearDown]
        public void GeneratePersonTestTearDown()
        {
            TestPerson = null;
        }

    }
}
