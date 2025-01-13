using System.Text.Json.Serialization;

namespace DeadLockAdmin.Models
{
    public class Character
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }

    public class ApiResponse
    {
        [JsonPropertyName("Персонажи")]
        public List<Character> Персонажи { get; set; }
    }

}
