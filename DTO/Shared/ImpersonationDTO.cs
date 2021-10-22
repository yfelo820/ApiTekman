using Newtonsoft.Json;

namespace Api.DTO.Shared
{
    public class ImpersonationDTO
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}