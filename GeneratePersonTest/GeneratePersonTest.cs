using GeneratePerson;
using NUnit.Framework;
using System;
using System.Linq;
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
        public void ShouldGenerateDifferentGenders()
        {
            var maleCount = 0;
            var femaleCount = 0;
            for (int x = 0; x < 100000; ++x)
            {
                TestPerson.GenerateGender();

                if (TestPerson.Gender == "Male")
                    maleCount++;
                if (TestPerson.Gender == "Female")
                    femaleCount++;
            }
            Assert.That(femaleCount, Is.AtLeast(40000));
            Assert.That(maleCount, Is.AtLeast(40000));
        }

        [Test]
        public void ShouldHaveGeneratedDataForAllFields()
        {
            TestPerson.GenerateRandomData();

            Assert.That(TestPerson.Address, Is.Not.Null.Or.Empty);
            Assert.That(TestPerson.City, Is.Not.Null.Or.Empty);
            Assert.That(TestPerson.Email, Is.Not.Null.Or.Empty);
            Assert.That(TestPerson.FirstName, Is.Not.Null.Or.Empty);
            Assert.That(TestPerson.Gender, Is.Not.Null.Or.Empty);
            Assert.That(TestPerson.LastName, Is.Not.Null.Or.Empty);
            Assert.That(TestPerson.Phone, Is.Not.Null.Or.Empty);
            Assert.That(TestPerson.SocialSecurityNumber, Is.Not.Null.Or.Empty);
            Assert.That(TestPerson.Zipcode, Is.Not.Null.Or.Empty);
            Assert.That(TestPerson.BirthDate, Is.Not.Null.And.GreaterThanOrEqualTo(new DateTime(1910, 1, 1)));

        }

        [Test]
        public void ShouldGeneratePersonOver18()
        {

            for (int x = 0; x < 100000; ++x)
            {
                TestPerson.GenerateBirthDate(true);

                Assert.That(TestPerson.Age, Is.AtLeast(18));
            }
        }

        [Test]
        public void ShouldGenerateCorrectPIN()
        {
            for (int x = 0; x < 100000; ++x)
            {
                TestPerson.GenerateGender();
                TestPerson.GenerateBirthDate(false);
                TestPerson.GenerateSocialSecurityNumber();

                Assert.That(FixPINformat(TestPerson.SocialSecurityNumber), Is.True);
                Assert.That(CheckPIN(TestPerson.SocialSecurityNumber), Is.True);
            }
        }

        private static bool FixPINformat(string str)
        {
            var r = new Regex("[^0-9]");
            var r3 = new Regex(@"^\d{10}$");

            str = r.Replace(str, "");

            return (!r3.IsMatch(str)) ? false : true;
        }

        private static bool CheckPIN(string str)
        {
            int sum = 0;
            for (int x = 9; x >= 0; x--)
            {
                if (x % 2 == 1)
                    sum += (int)Char.GetNumericValue(str[x]);
                else
                {

                    int y = (int)Char.GetNumericValue(str[x]) * 2;
                    if (y > 9)
                    {
                        int[] list = y.ToString().Select(w => (int)Char.GetNumericValue(w)).ToArray();

                        foreach (int i in list)
                            sum += i;
                    }
                    else
                        sum += (int)Char.GetNumericValue(str[x]) * 2;
                }

            }
            return ((sum % 10) == 0);
        }

        [TearDown]
        public void GeneratePersonTestTearDown()
        {
            TestPerson = null;
        }

    }
}
