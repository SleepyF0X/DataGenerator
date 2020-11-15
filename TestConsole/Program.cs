using System;
using DataGenerator;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder<Student> config = ConfigurationBuilder<Student>.GetBuilder()
                .ForProperty(s => s.Name, opt => opt.IsName)
                .ForProperty(s => s.Age, opt => opt.IsAge)
                .ForProperty(s => s.Surname, opt => opt.IsSurname)
                .ForProperty(s => s.Email, opt => opt.IsEmail);
            var models = new ModelBuilder<Student>(config, 3).Build();
            foreach (var model in models)
            {
                Console.WriteLine(model.Name + " " + model.Surname + " " + model.Age + " " + model.Email);
            }
        }
    }
}
