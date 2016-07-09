using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FluentValidation.Configuration.Tests.TestResources
{
    public class Person
    {
        public string NameField;
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }

        public List<Person> Children { get; set; }
        public string[] NickNames { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int? NullableInt { get; set; }

        public Person()
        {
            Children = new List<Person>();
            Orders = new List<Order>();
        }

        public int CalculateSalary()
        {
            return 20;
        }

        public Address Address { get; set; }
        public IList<Order> Orders { get; set; }

        public string Email { get; set; }
        public decimal Discount { get; set; }
        public double Age { get; set; }

        public int AnotherInt { get; set; }

        public string CreditCard { get; set; }

        public int? OtherNullableInt { get; set; }

        public string Regex { get; set; }

        public Regex AnotherRegex { get; set; }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public EnumGender Gender { get; set; }
    }
}
