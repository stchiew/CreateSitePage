namespace CreateSitePage.Helper
{
  using System.Collections.Generic;
  using System.Text.Json;
  using CreateSitePage.Models;

  public static class SerializerHelper
  {

    public static void Serialize()
    {
      List<Person> persons = new List<Person>();
      var person = new Person
      {
        Id = "id",
        Upn = "upn",
        Role = "role",
        Department = "dept",
        Phone = "phone",
        Sip = "sip"
      };
      persons.Add(person);
      var person1 = new Person
      {
        Id = "id",
        Upn = "upn",
        Role = "role",
        Department = "dept",
        Phone = "phone",
        Sip = "sip"
      };
      persons.Add(person1);
      var people = new People
      {
        Layout = 1,
        Persons = persons,
        Title = "Team Members"
      };
      var jsonstring = JsonSerializer.Serialize<People>(people);
    }
  }
}