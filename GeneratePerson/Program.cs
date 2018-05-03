using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePerson
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person();

            while (true) ;
        }
    }

    class Person
    {
        bool IsMale { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime BirthDate { get; set; }
        string SocialSecurityNumber { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string Zipcode { get; set; }
        string Phone { get; set; }
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

        public void ToJson()
        {
            // return jsonobject of this instance
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
