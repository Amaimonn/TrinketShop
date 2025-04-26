using Newtonsoft.Json.Linq;

namespace TrinketShop.Infrastructure.Migrations
{
    public class MigrationV1ToV2 : IMigration
    {
        public int ToVersion { get; } = 2;

        public JObject Migrate(JObject data)
        {
            return data;
        }
    }
}