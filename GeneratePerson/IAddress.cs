namespace GeneratePerson
{
    public interface IAddress
    {
        string Street { get; }

        string Zip { get; }

        string City { get; }

        void GenerateAddress();

    }
}