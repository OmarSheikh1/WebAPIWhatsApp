using Newtonsoft.Json;

namespace WebAPIWhatsApp
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Campaign
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Channel
    {
        public string code { get; set; }
        public string id { get; set; }
    }

    public class Menu
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Metadata
    {
    }

    public class Responses
    {
        public string loop { get; set; }
        public string ProspectiveMainMenu { get; set; }
        public string ContactType { get; set; }
        public string ReturningStudentMainMenu { get; set; }

        [JsonProperty("Main Menu")]
        public string MainMenu { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Assistance { get; set; }
        public string LearningCentre { get; set; }
    }

    public class Root
    {
        public object queryParams { get; set; }
        public int session { get; set; }
        public string msisdn { get; set; }
        public int time { get; set; }
        public Channel channel { get; set; }
        public Campaign campaign { get; set; }
        public Menu menu { get; set; }
        public object attachment { get; set; }
        public string enteredCode { get; set; }
        public Responses responses { get; set; }
        public Metadata metadata { get; set; }
    }


}
