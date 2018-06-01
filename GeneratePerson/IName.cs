namespace GeneratePerson
{
    public interface IName
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string FormattedName { get; }

        string FirstNameWithoutDiacretics { get; }

        string LastNameWithoutDiacretics { get; }

        void GenerateName(bool isMale);
    }
}