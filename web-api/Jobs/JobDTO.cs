using System.Text.Json.Serialization;
using web_api.Model;

namespace web_api.Jobs
{
    public class JobDTO
    {
        public Guid Id { get; set; }

        public string Status { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Error { get; set; } = null;

        public string Message { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProgressDTO? Progress { get; set; } = new ProgressDTO();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Queue<IPInfoEntity>? ProcessQueue { get; set; } = null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<IPInfoEntity>? Result { get; set; } = null;
    }
}
