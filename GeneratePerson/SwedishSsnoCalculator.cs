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

        //generate random valid swedish socialsecuritynumber
        public void GenerateSocialSecurityNumber(bool isMale, DateTime birthDate)
        {

            var x = _rnd.Next(10);
            if (isMale && (x % 2 == 0))
                x++;
            else if (!isMale && x % 2 != 0)
                x--;

            var ssno = birthDate.Year.ToString().Substring(2, 2) + birthDate.Month.ToString().PadLeft(2, '0') +
                          birthDate.Day.ToString().PadLeft(2, '0') + _rnd.Next(100).ToString().PadLeft(2, '0') + x;

            var sb = new StringBuilder();

            for (var i = 0; i < ssno.Length; ++i)
            {
                if (i % 2 == 0)
                    sb.Append(((int)char.GetNumericValue(ssno[i]) * 2).ToString());
                else
                    sb.Append(ssno[i]);
            }

            var result = sb.ToString().Sum(i => (int)char.GetNumericValue(i));

            ssno += ((10 - (result % 10)) % 10).ToString();

            SocialSecurityNumber = ssno;
        }
    }
}
