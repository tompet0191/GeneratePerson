using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;

namespace GeneratePerson
{

    public class Person : IXmlSerializable
    {
        private bool IsMale { get; set; }

        private Random Rnd { get; }

        private List<string> _maleNames;

        private List<string> _femaleNames;

        private List<string> _familyNames;

        private List<PostCode> _postCodes;

        private string WorkDir { get; }

        [JsonProperty]
        public string FirstName { get; set; }

        [JsonProperty]
        public string LastName { get; set; }

        [JsonProperty]
        public DateTime BirthDate { get; set; }

        [JsonProperty]
        public string SocialSecurityNumber { get; set; }

        [JsonProperty]
        public string Address { get; set; }

        [JsonProperty]
        public string City { get; set; }

        [JsonProperty]
        public string Zipcode { get; set; }

        [JsonProperty]
        public string Phone { get; set; }

        [JsonProperty]
        public string Email { get; set; }

        private class PostCode
        {
            public string Zip { get; set; }
            public string City { get; set; }
        }


        public Person()
        {
            WorkDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            Rnd = new Random();
            LoadJson();

            GenerateGender();
            GenerateName();
            GenerateBirthDate(false);
            GenerateSocialSecurityNumber();
            GenerateAddress();
            GeneratePhone();
            GenerateEmail();
        }

        public Person(string workDir)
        {
            Rnd = new Random();
            WorkDir = workDir;
            LoadJson();
        }

        public Person(bool? isMale)
        {
            WorkDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            Rnd = new Random();

            if (!isMale.HasValue)
                GenerateGender();
            else
                IsMale = (bool) isMale;
            
            LoadJson();

            GenerateName();
            GenerateBirthDate(false);
            GenerateSocialSecurityNumber();
            GenerateAddress();
            GeneratePhone();
            GenerateEmail();
        }

        public Person(bool? isMale, bool? generateOver18)
        {
            WorkDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            Rnd = new Random();
            
            if (!isMale.HasValue)
                GenerateGender();
            else
                IsMale = (bool)isMale;

            var over18 = (generateOver18.HasValue) && (bool) generateOver18;

            LoadJson();

            GenerateName();
            GenerateBirthDate(over18);
            GenerateSocialSecurityNumber();
            GenerateAddress();
            GeneratePhone();
            GenerateEmail();
        }

        public override string ToString()
        {
            return "";
        }

        public void LoadJson()
        {
            using (var r = new StreamReader(Path.Combine(WorkDir, @"..\..\lists\swedish_male_names.json")))
            {
                var json = r.ReadToEnd();
                _maleNames = JsonConvert.DeserializeObject<List<string>>(json);
            }

            using (var r = new StreamReader(Path.Combine(WorkDir, @"..\..\lists\swedish_female_names.json")))
            {
                var json = r.ReadToEnd();
                _femaleNames = JsonConvert.DeserializeObject<List<string>>(json);
            }

            using (var r = new StreamReader(Path.Combine(WorkDir, @"..\..\lists\swedish_family_names.json")))
            {
                var json = r.ReadToEnd();
                _familyNames = JsonConvert.DeserializeObject<List<string>>(json);
            }

            using (var r = new StreamReader(Path.Combine(WorkDir, @"..\..\lists\postcodes.json")))
            {
                var json = r.ReadToEnd();
                _postCodes = JsonConvert.DeserializeObject<List<PostCode>>(json);
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
        public void GenerateGender()
        {
            if (Rnd.Next(100) > 49)
                IsMale = true;
        }

        //sets first and lastname of this instance randomly
        public void GenerateName()
        {

            FirstName = IsMale ? _maleNames[Rnd.Next(_maleNames.Count)] : _femaleNames[Rnd.Next(_femaleNames.Count)];
            LastName = _familyNames[Rnd.Next(_familyNames.Count)];
        }

        //generate random birthdate
        public void GenerateBirthDate(bool over18)
        {
            var startDate = new DateTime(1940, 1, 1);
            var range = (over18) ? (DateTime.Today.AddYears(-18) - startDate).Days : (DateTime.Today - startDate).Days;

            BirthDate = startDate.AddDays(Rnd.Next(range));
        }

        //generate random valid swedish socialsecuritynumber
        public void GenerateSocialSecurityNumber()
        {
            var x = Rnd.Next(10);
            if (IsMale && (x % 2 == 0))
                x++;
            else if (!IsMale && x % 2 != 0)
                x--;

            var ssno = BirthDate.Year.ToString().Substring(2, 2) + BirthDate.Month.ToString().PadLeft(2, '0') +
                          BirthDate.Day.ToString().PadLeft(2, '0') + Rnd.Next(100).ToString().PadLeft(2, '0') + x;

            var sb = new StringBuilder();

            for (var i = 0; i < ssno.Length; ++i)
            {
                if (i % 2 == 0)
                    sb.Append( ((int)char.GetNumericValue(ssno[i]) * 2).ToString() );
                else
                    sb.Append(ssno[i]);
            }

            var result = sb.ToString().Sum(i => (int)char.GetNumericValue(i));

            ssno += ((10 - (result % 10)) % 10).ToString();
            
            SocialSecurityNumber = ssno;
        }

        //generate random address
        public void GenerateAddress()
        {
            var prefix = new[] { "Mönster", "Drottning", "Kungs", "Ny", "Gammel", "Lingon", "Oskar", "Kulla", "Regerings", "Norrlands", "Skåne", "Dala", "Stock", "Gryning", "Hallon", "Gotlands", "Professor", "Skräddar", "Präst", "Kammar", "Kyrko", "Timmer", "Stor", "Industri", "Riddar", "Ulvsunda", "Strand", "Ankar", "Bastu", "Balders", "Biblioteks", "Brunns", "Ersta", "Guld", "Karla", "Körsbärs", "Malm", "Ring", "Stall", "Vinter" };
            var postfix = new[] { "stigen", "vägen", "slingan", "gatan", "gränd" };

            Address = prefix[Rnd.Next(prefix.Length)] + postfix[Rnd.Next(postfix.Length)] + " " + Rnd.Next(100);

            //Gets first 2 numbers in zipcode which also determines the city
            var randPostcode = _postCodes[Rnd.Next(_postCodes.Count)];
            City = randPostcode.City;
            Zipcode = randPostcode.Zip;

            //Gets third number, for street address
            var thirdNumber = new[] { '2', '3', '4', '6', '7' };
            Zipcode += thirdNumber[Rnd.Next(thirdNumber.Length)];

            //Gets final numbers
            Zipcode += " " + Rnd.Next(100).ToString().PadLeft(2, '0'); //rand 00-99
        }

        public void GeneratePhone()
        {
            //generate random phone number
            Phone = "070-" + Rnd.Next(1000).ToString().PadLeft(3, '0') + " " + Rnd.Next(100).ToString().PadLeft(2, '0') + " " + Rnd.Next(100).ToString().PadLeft(2, '0');
        }

        public void GenerateEmail()
        {
            var domains = new[] { "whyspam.me", "trash-mail.com", "tempemail.com", "spamcowboy.com", "sendspamhere.com", "sogetthis.com", "netmails.net", "keepmymail.com", "hatespam.org", "iheartspam.org", "fastmazda.com", "discardmail.com", "10minutemail.com", "4warding.net", "deadaddress.com" };

            //generate random email address
            Email = RemoveDiacretics(FirstName).Replace(" ", "") + "." + RemoveDiacretics(LastName).Replace(" ", "") + "@" + domains[Rnd.Next(domains.Length)];
        }

        protected string GetFormattedName()
        {
            return FirstName + " " + LastName;
        }

        protected string RemoveDiacretics(string s)
        {
            s = s.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in s)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString();
        }

        XmlSchema IXmlSerializable.GetSchema() { return null; }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            FirstName = reader.ReadElementString("FirstName");
            LastName = reader.ReadElementString("LastName");
            BirthDate = DateTime.ParseExact(reader.ReadElementString("BirthDate"), "yyyy-MM-dd", null);
            SocialSecurityNumber = reader.ReadElementString("SocialSecurityNumber");
            Address = reader.ReadElementString("Address");
            City = reader.ReadElementString("City");
            Zipcode = reader.ReadElementString("Zipcode");
            Phone = reader.ReadElementString("Phone");
            Email = reader.ReadElementString("Email");
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("FirstName", FirstName);
            writer.WriteElementString("LastName", LastName);
            writer.WriteElementString("BirthDate", BirthDate.ToString("yyyy-MM-dd"));
            writer.WriteElementString("SocialSecurityNumber", SocialSecurityNumber);
            writer.WriteElementString("Address", Address);
            writer.WriteElementString("City", City);
            writer.WriteElementString("Zipcode", Zipcode);
            writer.WriteElementString("Phone", Phone);
            writer.WriteElementString("Email", Email);
        }
    }
}
