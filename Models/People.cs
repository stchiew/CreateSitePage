// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CreateSitePage.Models
{


  public class Person
  {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("upn")]
    public string Upn { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("department")]
    public string Department { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("sip")]
    public string Sip { get; set; }
  }

  public class People
  {
    [JsonPropertyName("layout")]
    public int Layout { get; set; }

    [JsonPropertyName("persons")]
    public List<Person> Persons { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
  }
}
