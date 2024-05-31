using CsvDataApp.Models;
using CsvHelper.Configuration;

namespace CsvDataApp.Helpers.CsvHelper
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Map(m => m.Identity).Name("Identity");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.Sirname).Name("Surname"); // Map SurName in CSV to Sirname in model
            Map(m => m.Age).Name("Age");
            Map(m => m.Sex).Name("Sex");
            Map(m => m.Mobile).Name("Mobile");
            Map(m => m.Active).Name("Active");
        }
    }
}
