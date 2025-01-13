using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DeadLockAdmin.Models
{
    public class Build
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }
    }

    public class BuildResponse
    {
        [JsonPropertyName("character")]
        public Character Character { get; set; }

        [JsonPropertyName("builds")]
        public List<Build> Builds { get; set; }
    }
}
