using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataGenerator
{
    public class OptionBuilder
    {
        private readonly string[] _names = File.ReadAllLines(@"D:\.NET\Repo\DataGenerator\DataGenerator\Resources\NamesEn.txt");
        private readonly string[] _surnames = File.ReadAllLines(@"D:\.NET\Repo\DataGenerator\DataGenerator\Resources\SurnamesEn.txt");
        private readonly string[] _emails = File.ReadAllLines(@"D:\.NET\Repo\DataGenerator\DataGenerator\Resources\Emails.txt");
        private Func<dynamic> _function;
        public Func<dynamic> IsName()
        {
            _function = () =>
            {
                var name = _names[new Random().Next(_names.Length)];
                return name;
            };
            return _function;
        }
        public Func<dynamic> IsSurname()
        {
            _function = () =>
            {
                var surname = _surnames[new Random().Next(_surnames.Length)];
                return surname;
            };
            return _function;
        }
        public Func<dynamic> IsAge()
        {
            _function = () =>
            {
                var rnd = new Random();
                var name = rnd.Next(10, 75); ;
                return name;
            };
            return _function;
        }
        public Func<dynamic> IsEmail()
        {
            _function = () =>
            {
                var email = _emails[new Random().Next(_emails.Length)];
                return email;
            };
            return _function;
        }
    }
}
