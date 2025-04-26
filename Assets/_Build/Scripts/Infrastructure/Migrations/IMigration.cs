using Newtonsoft.Json.Linq;

namespace TrinketShop.Infrastructure.Migrations
{
    public interface IMigration
    {
        public int ToVersion { get; }
        public JObject Migrate(JObject dataObject);
    }
}