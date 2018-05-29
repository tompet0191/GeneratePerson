﻿using GeneratePerson;
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
