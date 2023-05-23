using System.Text.Json;

namespace BtzjManagement.Api.Filter
{
    public class LowercaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToLower();
        }
    }
}
