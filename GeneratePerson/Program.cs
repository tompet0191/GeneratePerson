using System;
using Newtonsoft.Json;

namespace GeneratePerson
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person();

            Console.WriteLine(p.ToFormattedJson());

            while (true) ;
        }
    }

    class Person
    {   
        [JsonProperty]
        bool IsMale { get; set; }

        [JsonProperty]
        string FirstName { get; set; }

        [JsonProperty]
        string LastName { get; set; }

        [JsonProperty]
        DateTime BirthDate { get; set; }

        [JsonProperty]
        string SocialSecurityNumber { get; set; }

        [JsonProperty]
        string Address { get; set; }

        [JsonProperty]
        string City { get; set; }

        [JsonProperty]
        string Zipcode { get; set; }

        [JsonProperty]
        string Phone { get; set; }

        [JsonProperty]
        string Email { get; set; }

        public Person()
        {
            //generate a random person by calling functions to randomly generate each property.
        }

        override public string ToString()
        {
            //return string object of this instance
            return "";
        }

        public string ToJson()
        {
            // return jsonobject of this instance
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" });
        }

        public string ToFormattedJson()
        {
            return JsonConvert.DeserializeObject(this.ToJson()).ToString();
        }

        public void ToXml()
        {
            //return xml of this instance
        }

        protected void GenerateGender()
        {
            //sets the gender of the instance randomly
        }
        protected void GenerateName(bool isMale)
        {
            //sets first and lastname of this instance randomly
        }

        protected void GenerateBirthDate()
        {
            //generate random birthdate
        }

        protected void GenerateSocialSecurityNumber(bool isMale, DateTime birthDate)
        {
            //generate random socialsecuritynumber
        }

        protected void GenerateAddress()
        {
            //generate random address
        }

        protected void GeneratePhone()
        {
            //generate random phone number
        }
        
        protected void GenerateEmail(string firstName, string lastName)
        {
            //generate random email address
        }
    }
}
