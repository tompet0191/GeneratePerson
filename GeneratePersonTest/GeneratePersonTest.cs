using GeneratePerson;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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

            for (int x = 0; x < 100000; ++x)
            {
                TestPerson.GenerateBirthDate(true);

                birthdates.Add(TestPerson.BirthDate);
            }

            foreach (var bd in birthdates)
            {
                int age = DateTime.Now.Year - bd.Year;
                if ((DateTime.Now.DayOfYear < bd.DayOfYear) && age <= 18)
                    age--;

                Assert.That(age, Is.AtLeast(18));
            }
        }

        [TearDown]
        public void GeneratePersonTestTearDown()
        {
            TestPerson = null;
        }

    }
}
