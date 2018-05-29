using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GeneratePerson
{
    public class Name
    {
        private readonly Random _rnd;

        private readonly List<string> _maleNames;

        private readonly List<string> _femaleNames;

        private readonly List<string> _familyNames;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FormattedName => FirstName + " " + LastName;

        public Name(string workDir, Random rnd)
        {
            _rnd = rnd;
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
    }
}