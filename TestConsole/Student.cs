using System;
using System.Collections.Generic;
using System.Text;

namespace TestConsole
{
    public class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public int RecordBook { get; set; }
        public string Info { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
    }
}
