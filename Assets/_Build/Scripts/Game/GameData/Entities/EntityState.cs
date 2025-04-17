using TrinketShop.Game.GameData;

namespace TrinketShop.GameData.Entities
{
    public abstract class EntityState : SaveState<EntityState>
    {
        public string Id;
        public string TypeId;
    }
}