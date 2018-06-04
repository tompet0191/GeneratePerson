using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace GeneratePerson
{

    public class Person : IXmlSerializable
    {
        private readonly IAddress _address;
        private readonly ISsnoCalculator _ssnoCalculator;
        private readonly IName _name;

        private readonly Random _rnd;

        private bool _isMale;

        [JsonProperty]
        public DateTime BirthDate { get; private set; }

        [JsonProperty]
        public string Phone { get; private set; }

        [JsonProperty]
        public string Email { get; private set; }

        [JsonProperty]
        public string SocialSecurityNumber => _ssnoCalculator.SocialSecurityNumber;

        [JsonProperty]
        public string FirstName => _name.FirstName;

        [JsonProperty]
        public string LastName => _name.LastName;

        [JsonProperty]
        public string FormattedName => _name.FormattedName;

        [JsonProperty]
        public string Gender => (_isMale) ? "Male" : "Female";

        [JsonProperty]
        public int Age
        {
            get
            {
                var timeSpan = DateTime.Now - BirthDate;
                return timeSpan.Days / 365;
            }
        }

        [JsonProperty]
        public string Address => _address.Street;

        [JsonProperty]
        public string City => _address.City;

        [JsonProperty]
        public string Zipcode => _address.Zip;

        private Person()
        {
            _rnd = new Random();
        }

        public Person(IName name, IAddress address, ISsnoCalculator ssnoCalculator)
        {
            _rnd = new Random();
            var workDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            _address = address;
            _name = name;
            _ssnoCalculator = ssnoCalculator;

            GenerateRandomData();
        }

        public Person(IName name, IAddress address, ISsnoCalculator ssnoCalculator, string workDir)
        {
            _rnd = new Random();
            _address = address;
            _name = name;
            _ssnoCalculator = ssnoCalculator;
        }

        public Person(IName name, IAddress address, ISsnoCalculator ssnoCalculator, bool isMale)
            : this(name, address, ssnoCalculator)
        {

            _isMale = isMale;

            _name.GenerateName(_isMale);
            _ssnoCalculator.GenerateSocialSecurityNumber(_isMale, BirthDate);
            GeneratePhone();
            GenerateEmail();
        }

        public Person(IName name, IAddress address, ISsnoCalculator ssnoCalculator, bool? isMale, bool generateOver18)
            : this(name, address, ssnoCalculator)
        {

            _isMale = isMale ?? _rnd.Next(0, int.MaxValue) % 2 == 0; //Generate gender

            _name.GenerateName(_isMale);
            GenerateBirthDate(generateOver18);
            _ssnoCalculator.GenerateSocialSecurityNumber(_isMale, BirthDate);
            GeneratePhone();
            GenerateEmail();
        }

        public void GenerateRandomData()
        {
            GenerateGender();
            _name.GenerateName(_isMale);
            GenerateBirthDate(false);
            _ssnoCalculator.GenerateSocialSecurityNumber(_isMale, BirthDate);
            _address.GenerateAddress();
            GeneratePhone();
            GenerateEmail();
        }

        //sets the gender of the instance randomly
        public void GenerateGender()
        {
            _isMale = _rnd.Next(0, int.MaxValue) % 2 == 0;
        }

        //generate random birthdate
        public void GenerateBirthDate(bool over18)
        {
            var startDate = new DateTime(1940, 1, 1);
            var range = (over18) ? (DateTime.Today.AddYears(-18) - startDate).Days : (DateTime.Today - startDate).Days;

            BirthDate = startDate.AddDays(_rnd.Next(range));
        }

        public void GenerateAddress()
        {
            _address.GenerateAddress();
        }

        public void GenerateSocialSecurityNumber()
        {
            _ssnoCalculator.GenerateSocialSecurityNumber(_isMale, BirthDate);
        }

        public void GeneratePhone()
        {
            //generate random phone number
            Phone = "070-" + _rnd.Next(1000).ToString().PadLeft(3, '0') + " " + _rnd.Next(100).ToString().PadLeft(2, '0') + " " + _rnd.Next(100).ToString().PadLeft(2, '0');
        }

        public void GenerateEmail()
        {
            var domains = new [] { "whyspam.me", "trash-mail.com", "tempemail.com", "spamcowboy.com", "sendspamhere.com", "sogetthis.com", "netmails.net", "keepmymail.com", "hatespam.org", "iheartspam.org", "fastmazda.com", "discardmail.com", "10minutemail.com", "4warding.net", "deadaddress.com" };

            //generate random email address. note that name should always be set.
            Email = _name.FirstNameWithoutDiacretics.Replace(" ", "") + "." + _name.LastNameWithoutDiacretics.Replace(" ", "") + "@" + domains[_rnd.Next(domains.Length)];
        }

        public override string ToString()
        {
            throw new NotImplementedException();
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

        XmlSchema IXmlSerializable.GetSchema() { return null; }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
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
