using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace GeneratePerson
{
    public class Person : IXmlSerializable
    {
        private bool IsMale { get; set; }

        private Random Rnd { get; set; }

        private List<string> _maleNames;

        private List<string> _femaleNames;

        private List<string> _familyNames;

        [JsonProperty]
        private string FirstName { get; set; }

        [JsonProperty]
        private string LastName { get; set; }

        [JsonProperty]
        private DateTime BirthDate { get; set; }

        [JsonProperty]
        private string SocialSecurityNumber { get; set; }

        [JsonProperty]
        private string Address { get; set; }

        [JsonProperty]
        private string City { get; set; }

        [JsonProperty]
        private string Zipcode { get; set; }

        [JsonProperty]
        private string Phone { get; set; }

        [JsonProperty]
        private string Email { get; set; }

        public Person()
        {
            Rnd = new Random();
            LoadJson();

            GenerateGender();
            GenerateName();
            GenerateBirthDate();
            GenerateSocialSecurityNumber();
            GenerateAddress();
            GeneratePhone();
            GenerateEmail();

        }

        public Person(bool isMale)
        {
            IsMale = isMale;
            Rnd = new Random();
            LoadJson();

            GenerateName();
            GenerateBirthDate();
            GenerateSocialSecurityNumber();
            GenerateAddress();
            GeneratePhone();
            GenerateEmail();
        }

        public override string ToString()
        {
            return "";
        }

        private void LoadJson()
        {
            using (var r = new StreamReader("../../lists/swedish_male_names.json"))
            {
                var json = r.ReadToEnd();
                _maleNames = JsonConvert.DeserializeObject<List<string>>(json);
            }

            using (var r = new StreamReader("../../lists/swedish_female_names.json"))
            {
                var json = r.ReadToEnd();
                _femaleNames = JsonConvert.DeserializeObject<List<string>>(json);
            }

            using (var r = new StreamReader("../../lists/swedish_family_names.json"))
            {
                var json = r.ReadToEnd();
                _familyNames = JsonConvert.DeserializeObject<List<string>>(json);
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" });
        }

        public string ToFormattedJson()
        {
            return JsonConvert.DeserializeObject(this.ToJson()).ToString();
        }

        public string ToXml()
        {
            var x = new XmlSerializer(this.GetType());
            using (var textWriter = new StringWriter())
            {
                x.Serialize(textWriter, this);
                return textWriter.ToString();
            }
        }

        //sets the gender of the instance randomly
        protected void GenerateGender()
        {
            if (Rnd.Next(100) > 49)
                this.IsMale = true;
        }

        //sets first and lastname of this instance randomly
        // json-files containing names from https://github.com/bolddp/swedish-names
        protected void GenerateName()
        {
            FirstName = IsMale ? _maleNames[Rnd.Next(_maleNames.Count)] : _femaleNames[Rnd.Next(_femaleNames.Count)];
            LastName = _familyNames[Rnd.Next(_familyNames.Count)];
        }

        //generate random birthdate
        protected void GenerateBirthDate()
        {
            var startDate = new DateTime(1940, 1, 1);
            var range = (DateTime.Today - startDate).Days;
            BirthDate = startDate.AddDays(Rnd.Next(range));
        }

        //generate random valid swedish socialsecuritynumber
        protected void GenerateSocialSecurityNumber()
        {
            var x = Rnd.Next(10);
            if (IsMale && (x % 2 == 0))
                x++;
            else if (!IsMale && x % 2 != 0)
                x--;

            var ssno = BirthDate.Year.ToString().Substring(2, 2) + BirthDate.Month.ToString().PadLeft(2, '0') +
                          BirthDate.Day.ToString().PadLeft(2, '0') + Rnd.Next(100).ToString().PadLeft(2, '0') + x.ToString();

            var total = "";

            for (var i = 0; i < ssno.Length; ++i)
            {
                if (i % 2 == 0)
                    total += ((int)char.GetNumericValue(ssno[i]) * 2).ToString();
                else
                    total += ssno[i];
            }

            var result = total.Sum(i => (int)char.GetNumericValue(i));

            ssno += ((10 - (result % 10)) % 10).ToString();

            SocialSecurityNumber = ssno;
        }

        //generate random address
        protected void GenerateAddress()
        {

        }

        protected void GeneratePhone()
        {
            //generate random phone number
        }

        protected void GenerateEmail()
        {
            //generate random email address
            Email = FirstName.Replace(" ", "") + "." + LastName + "@" + "randomdomain" + ".se";
        }

        XmlSchema IXmlSerializable.GetSchema() { return null; }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            this.FirstName = reader.ReadElementString("FirstName");
            this.LastName = reader.ReadElementString("LastName");
            this.BirthDate = DateTime.ParseExact(reader.ReadElementString("BirthDate"), "yyyy-MM-dd", null);
            this.SocialSecurityNumber = reader.ReadElementString("SocialSecurityNumber");
            this.Address = reader.ReadElementString("Address");
            this.City = reader.ReadElementString("City");
            this.Zipcode = reader.ReadElementString("Zipcode");
            this.Phone = reader.ReadElementString("Phone");
            this.Email = reader.ReadElementString("Email");
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("FirstName", this.FirstName);
            writer.WriteElementString("LastName", this.LastName);
            writer.WriteElementString("BirthDate", this.BirthDate.ToString("yyyy-MM-dd"));
            writer.WriteElementString("SocialSecurityNumber", this.SocialSecurityNumber);
            writer.WriteElementString("Address", this.Address);
            writer.WriteElementString("City", this.City);
            writer.WriteElementString("Zipcode", this.Zipcode);
            writer.WriteElementString("Phone", this.Phone);
            writer.WriteElementString("Email", this.Email);
        }
    }
}
