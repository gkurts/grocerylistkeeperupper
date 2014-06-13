using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GroceryListKeeperUpper
{
    public class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            this.ContractResolver = new CamelCasePropertyNamesContractResolver();
            this.Formatting = Formatting.Indented;
        }
    }
}