using TrinketShop.Game.GameData;

namespace TrinketShop.Infrastructure.Providers
{
    [System.Serializable]
    public abstract class BaseGameState : IVersioned
    {
        public int Version => 1;
    }
}