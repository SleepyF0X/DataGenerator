using System;
using System.Collections.Generic;
using DataGenerator;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Group group = new Group
            {
                Id=Guid.NewGuid(),
                Name="IA-83"
            };
            ConfigurationBuilder<Student> config = ConfigurationBuilder<Student>.GetBuilder()
                .ForProperty(s => s.Id, opt => opt.IsGuid())
                .ForProperty(s => s.Name, opt => opt.IsName())
                .ForProperty(s => s.Age, opt => opt.IsAge())
                .ForProperty(s => s.Surname, opt => opt.IsSurname())
                .ForProperty(s => s.Email, opt => opt.IsEmail())
                .ForProperty(s => s.RecordBook, opt => opt.IsRndInt(1000, 9999))
                .ForProperty(s => s.Info, opt => opt.IsRndText(10));
            var models = new ModelBuilder<Student>(config.WithParent(s => s.GroupId, s => s.Group, g => g.Id, group)).Build(100);
            foreach (var model in models)
            {
                Console.WriteLine("Id: " + model.Id + Environment.NewLine + 
                                  "Name: " + model.Name + Environment.NewLine + 
                                  "Surname: " + model.Surname + Environment.NewLine +
                                  "Age: " + model.Age + Environment.NewLine +
                                  "Email: " + model.Email + Environment.NewLine +
                                  "Record Book number: " + model.RecordBook + Environment.NewLine +
                                  "Info: " + model.Info + Environment.NewLine +
                                  "GroupId: " + model.GroupId+ Environment.NewLine +
                                  "Group Name: " + model.Group?.Name + Environment.NewLine +
                                  "___________________________________________________________________");
            }
        }
    }
}
