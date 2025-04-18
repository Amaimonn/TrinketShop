using TrinketShop.Game.World.Trinkets;

namespace TrinketShop.Game.GameData.Entities.Trinket
{
    public interface ITrinketLevelConfig 
    {
        public uint Level { get; }
        public TrinketEntity Prefab { get; }
        public uint BaseIncome { get; }
        public float PassiveIncomeCooldown { get; }
    }
}