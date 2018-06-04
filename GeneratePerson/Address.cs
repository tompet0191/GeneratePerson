using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GeneratePerson
{

    public class Address : IAddress
    {
        private readonly Random _rnd;
        
        private readonly List<PostCode> _postCodes;

        public string Street { get; private set; }

        public string Zip { get; private set; }

        public string City { get; private set; }

        public Address()
        {
            _rnd = new Random();

            var json = "[ { \"Zip\": \"11\", \"City\": \"Stockholm\" }, { \"Zip\": \"21\", \"City\": \"Malm�\" }, { \"Zip\": \"22\", \"City\": \"Lund\" }, { \"Zip\": \"25\", \"City\": \"Helsingborg\" }, { \"Zip\": \"30\", \"City\": \"Halmstad\" }, { \"Zip\": \"35\", \"City\": \"V�xj�\" }, { \"Zip\": \"39\", \"City\": \"Kalmar\" }, { \"Zip\": \"41\", \"City\": \"G�teborg\" }, { \"Zip\": \"50\", \"City\": \"Bor�s\" }, { \"Zip\": \"55\", \"City\": \"J�nk�ping\" }, { \"Zip\": \"58\", \"City\": \"Link�ping\" }, { \"Zip\": \"60\", \"City\": \"Norrk�ping\" }, { \"Zip\": \"62\", \"City\": \"Gotland\" }, { \"Zip\": \"63\", \"City\": \"Eskilstuna\" }, { \"Zip\": \"65\", \"City\": \"Karlstad\" }, { \"Zip\": \"70\", \"City\": \"�rebro\" }, { \"Zip\": \"72\", \"City\": \"V�ster�s\" }, { \"Zip\": \"75\", \"City\": \"Uppsala\" }, { \"Zip\": \"80\", \"City\": \"G�vle\" }, { \"Zip\": \"85\", \"City\": \"Sundsvall\" }, { \"Zip\": \"90\", \"City\": \"Ume�\" }, { \"Zip\": \"97\", \"City\": \"Lule�\" } ]";
            _postCodes = JsonConvert.DeserializeObject<List<PostCode>>(json);
        }

        public Address(string workDir)
        {
            _rnd = new Random();

            try
            {
                using (var r = new StreamReader(Path.Combine(workDir, @"..\..\lists\postcodes.json")))
                {
                    var json = r.ReadToEnd();
                    _postCodes = JsonConvert.DeserializeObject<List<PostCode>>(json);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found");
            }
           
        }

        public void GenerateAddress()
        {
            var prefix = new[] { "M�nster", "Drottning", "Kungs", "Ny", "Gammel", "Lingon", "Oskar", "Kulla", "Regerings", "Norrlands", "Sk�ne", "Dala", "Stock", "Gryning", "Hallon", "Gotlands", "Professor", "Skr�ddar", "Pr�st", "Kammar", "Kyrko", "Timmer", "Stor", "Industri", "Riddar", "Ulvsunda", "Strand", "Ankar", "Bastu", "Balders", "Biblioteks", "Brunns", "Ersta", "Guld", "Karla", "K�rsb�rs", "Malm", "Ring", "Stall", "Vinter" };
            var postfix = new[] { "stigen", "v�gen", "slingan", "gatan", "gr�nd" };

            Street = prefix[_rnd.Next(prefix.Length)] + postfix[_rnd.Next(postfix.Length)] + " " + _rnd.Next(100);

            var randPostcode = _postCodes[_rnd.Next(_postCodes.Count)];
            City = randPostcode.City;
            Zip = randPostcode.Zip;

            //Gets third number, for street address
            var thirdNumber = new[] { '2', '3', '4', '6', '7' };
            Zip += thirdNumber[_rnd.Next(thirdNumber.Length)];

            //Gets final numbers
            Zip += " " + _rnd.Next(100).ToString().PadLeft(2, '0'); //rand 00-99
        }

        private class PostCode
        {

            public string Zip { get; set; }
            public string City { get; set; }
        }
    }
}