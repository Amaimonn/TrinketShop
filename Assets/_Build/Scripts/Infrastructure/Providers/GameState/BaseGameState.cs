using TrinketShop.Game.GameData;

namespace TrinketShop.Infrastructure.Providers
{
    [System.Serializable]
    public abstract class BaseGameState : IVersioned
    {
        public abstract int Version { get; set; }
    }
}