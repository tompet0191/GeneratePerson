using System;

namespace GeneratePerson
{
    public interface ISsnoCalculator
    {
        string SocialSecurityNumber { get; set; }

        void GenerateSocialSecurityNumber(bool isMale, DateTime birthDate);
    }
}
