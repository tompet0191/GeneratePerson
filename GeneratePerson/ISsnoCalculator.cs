using System;

namespace GeneratePerson
{
    public interface ISsnoCalculator
    {
        string GenerateSocialSecurityNumber(bool isMale, DateTime birthDate);
    }
}
