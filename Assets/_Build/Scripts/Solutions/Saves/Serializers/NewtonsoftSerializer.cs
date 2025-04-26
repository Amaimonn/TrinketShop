using Newtonsoft.Json;

using TrinketShop.Solutions.Saves;

namespace LostKaiju.Services.Saves
{
    public class NewtonsoftSerializer : ISerializer
    {
        public string Serialize<T>(T rawData)
        {
            return JsonConvert.SerializeObject(rawData);
        }

        public T Deserialize<T>(string serializedData)
        {
            return JsonConvert.DeserializeObject<T>(serializedData);
        }
    }
}