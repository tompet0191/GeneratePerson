using System;
using System.Linq;
using System.Text;

namespace GeneratePerson
{
    public class SwedishSsnoCalculator : ISsnoCalculator
    {
        private readonly Random _rnd;

        public string SocialSecurityNumber { get; set; }

        public SwedishSsnoCalculator()
        {
            _rnd = new Random();
        }

        public void GenerateSocialSecurityNumber(bool isMale, DateTime birthDate)
        {
            
            var controlNumbers = GetControlNumbers(isMale);

            var formattedBirthDate = birthDate.Year.ToString().Substring(2, 2) +
                                     birthDate.Month.ToString().PadLeft(2, '0') +
                                     birthDate.Day.ToString().PadLeft(2, '0');

            var ssno = formattedBirthDate + controlNumbers;

            SocialSecurityNumber = ssno + CalculateLastControlNumber(ssno);

        }

        private string GetControlNumbers(bool isMale)
        {
            var controlNumber = _rnd.Next(100).ToString().PadLeft(2, '0');

            var thirdNumber = _rnd.Next(10);

            if (isMale && (thirdNumber % 2 == 0))
                thirdNumber++;
            else if (!isMale && thirdNumber % 2 != 0)
                thirdNumber--;

            return controlNumber + thirdNumber;
        }

        private static string CalculateLastControlNumber(string ssno)
        {
            var productsString = GetProductsFromString(ssno);

            var result = productsString.Sum(i => (int) char.GetNumericValue(i));

            return ((10 - (result % 10)) % 10).ToString();
        }

        private static string GetProductsFromString(string ssno)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < ssno.Length; ++i)
            {
                if (i % 2 == 0)
                    sb.Append(((int) char.GetNumericValue(ssno[i]) * 2).ToString());
                else
                    sb.Append(ssno[i]);
            }
            return sb.ToString();
        }
    }
}
