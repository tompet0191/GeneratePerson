using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace GeneratePerson
{

    public class Name : IName
    {
        private readonly Random _rnd;

        private readonly List<string> _maleNames;

        private readonly List<string> _femaleNames;

        private readonly List<string> _familyNames;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FormattedName => FirstName + " " + LastName;

        public string FirstNameWithoutDiacretics => RemoveDiacretics(FirstName);

        public string LastNameWithoutDiacretics => RemoveDiacretics(LastName);

        public Name()
        {
            _rnd = new Random();

            var json = "[ \"Adam\", \"Adrian\", \"Albin\", \"Alexander\", \"Alfred\", \"Ali\", \"Alvin\", \"Anton\", \"Aron\", \"Arvid\", \"August\", \"Axel\", \"Benjamin\", \"Carl\", \"Casper\", \"Charlie\", \"Colin\", \"Daniel\", \"David\", \"Ebbe\", \"Edvin\", \"Elias\", \"Elis\", \"Elliot\", \"Elton\", \"Elvin\", \"Emil\", \"Erik\", \"Felix\", \"Filip\", \"Folke\", \"Frank\", \"Frans\", \"Gabriel\", \"Gustav\", \"Harry\", \"Henry\", \"Hjalmar\", \"Hugo\", \"Isak\", \"Ivar\", \"Jack\", \"Jacob\", \"Joel\", \"John\", \"Jonathan\", \"Josef\", \"Julian\", \"Kevin\", \"Kian\", \"Leo\", \"Leon\", \"Levi\", \"Liam\", \"Loke\", \"Loui\", \"Love\", \"Lucas\", \"Ludvig\", \"Malte\", \"Matteo\", \"Max\", \"Maximilian\", \"Melker\", \"Melvin\", \"Milian\", \"Milo\", \"Milton\", \"Mohamed\", \"Neo\", \"Nicolas\", \"Nils\", \"Noah\", \"Noel\", \"Oliver\", \"Olle\", \"Omar\", \"Oscar\", \"Otto\", \"Sam\", \"Samuel\", \"Sebastian\", \"Sigge\", \"Simon\", \"Sixten\", \"Tage\", \"Theo\", \"Theodor\", \"Thor\", \"Walter\", \"Vidar\", \"Vide\", \"Viggo\", \"Viktor\", \"Vilgot\", \"Wilhelm\", \"Ville\", \"William\", \"Wilmer\", \"Vincent\" ]";
            _maleNames = JsonConvert.DeserializeObject<List<string>>(json);

            json = "[\"Alice\",\"Alicia\",\"Olivia\",\"Ella\",\"Ebba\",\"Lilly\",\"Astrid\",\"Saga\",\"Freja\",\"Wilma\",\"Maja\",\"Agnes\",\"Elsa\",\"Alma\",\"Clara\",\"Ellie\",\"Selma\",\"Julia\",\"Stella\",\"Alva\",\"Signe\",\"Vera\",\"Ellen\",\"Leah\",\"Molly\",\"Ines\",\"Ester\",\"Linnea\",\"Isabelle\",\"Sara\",\"Nova\",\"Nellie\",\"Emilia\",\"Emma\",\"Elvira\",\"Sigrid\",\"Iris\",\"Nora\",\"Lova\",\"Juni\",\"Sofia\",\"Edith\",\"Elise\",\"Celine\",\"Liv\",\"Elin\",\"Luna\",\"Livia\",\"Leia\",\"Isabella\",\"Tyra\",\"Maria\",\"Meja\",\"Lykke\",\"Tuva\",\"Hanna\",\"Felicia\",\"Thea\",\"Ingrid\",\"Majken\",\"Ida\",\"Sally\",\"Amelia\",\"Moa\",\"Cornelia\",\"Lovisa\",\"Stina\",\"Rut\",\"Melissa\",\"Matilda\",\"Joline\",\"Siri\",\"Jasmine\",\"Ronja\",\"Bianca\",\"Lo\",\"Svea\",\"Maryam\",\"Amanda\",\"Mila\",\"Tilde\",\"Filippa\",\"Penny\",\"Märta\",\"Cleo\",\"Hilda\",\"Hedda\",\"Hilma\",\"Emelie\",\"Hedvig\",\"Julie\",\"Mira\",\"Ellinor\",\"My\",\"Greta\",\"Lovis\",\"Zoey\",\"Idun\",\"Melina\",\"Noomi\"]";
            _femaleNames = JsonConvert.DeserializeObject<List<string>>(json);

            json = "[ \"Andersson\", \"Johansson\", \"Karlsson\", \"Nilsson\", \"Eriksson\", \"Larsson\", \"Olsson\", \"Persson\", \"Svensson\", \"Gustafsson\", \"Pettersson\", \"Jonsson\", \"Jansson\", \"Hansson\", \"Bengtsson\", \"Jönsson\", \"Lindberg\", \"Jakobsson\", \"Magnusson\", \"Olofsson\", \"Lindström\", \"Lindqvist\", \"Lindgren\", \"Axelsson\", \"Berg\", \"Bergström\", \"Lundberg\", \"Lundgren\", \"Lind\", \"Lundqvist\", \"Mattsson\", \"Berglund\", \"Fredriksson\", \"Sandberg\", \"Henriksson\", \"Forsberg\", \"Sjöberg\", \"Wallin\", \"Engström\", \"Eklund\", \"Danielsson\", \"Lundin\", \"Håkansson\", \"Björk\", \"Gunnarsson\", \"Bergman\", \"Holm\", \"Samuelsson\", \"Wikström\", \"Ali\", \"Fransson\", \"Isaksson\", \"Mohamed\", \"Bergqvist\", \"Nyström\", \"Arvidsson\", \"Holmberg\", \"Löfgren\", \"Söderberg\", \"Nyberg\", \"Blomqvist\", \"Claesson\", \"Nordström\", \"Mårtensson\", \"Lundström\", \"Eliasson\", \"Viklund\", \"Pålsson\", \"Björklund\", \"Berggren\", \"Sandström\", \"Lund\", \"Nordin\", \"Ström\", \"Åberg\", \"Hermansson\", \"Ekström\", \"Holmgren\", \"Ahmed\", \"Falk\", \"Hassan\", \"Hedlund\", \"Dahlberg\", \"Sundberg\", \"Hellström\", \"Sjögren\", \"Abrahamsson\", \"Ek\", \"Blom\", \"Martinsson\", \"Öberg\", \"Andreasson\", \"Månsson\", \"Strömberg\", \"Åkesson\", \"Hansen\", \"Norberg\", \"Jonasson\", \"Lindholm\", \"Sundström\" ]";
            _familyNames = JsonConvert.DeserializeObject<List<string>>(json);
        }
        public Name(string workDir)
        {
            _rnd = new Random();

            using (var r = new StreamReader(Path.Combine(workDir, @"..\..\lists\swedish_male_names.json")))
            {
                var json = r.ReadToEnd();
                _maleNames = JsonConvert.DeserializeObject<List<string>>(json);
            }

            using (var r = new StreamReader(Path.Combine(workDir, @"..\..\lists\swedish_female_names.json")))
            {
                var json = r.ReadToEnd();
                _femaleNames = JsonConvert.DeserializeObject<List<string>>(json);
            }

            using (var r = new StreamReader(Path.Combine(workDir, @"..\..\lists\swedish_family_names.json")))
            {
                var json = r.ReadToEnd();
                _familyNames = JsonConvert.DeserializeObject<List<string>>(json);
            }
        }

        public void GenerateName(bool isMale)
        {

            FirstName = isMale ? _maleNames[_rnd.Next(_maleNames.Count)] : _femaleNames[_rnd.Next(_femaleNames.Count)];
            LastName = _familyNames[_rnd.Next(_familyNames.Count)];
        }

        private string RemoveDiacretics(string s)
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
    }
}