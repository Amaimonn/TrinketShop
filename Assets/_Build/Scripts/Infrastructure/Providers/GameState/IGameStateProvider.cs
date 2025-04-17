using TrinketShop.Game.GameData.Currency;
using TrinketShop.Game.GameData.Map;
using TrinketShop.Game.GameData.Upgrades;

namespace TrinketShop.Infrastructure.Providers
{
    public interface IGameStateProvider
    {
        public CurrencyState CurrencyState { get; }
        public MapState MapState { get; }
        public UpgradesState UpgradesState { get; }
        public ShopState ShopState { get; }
        
        public void LoadAll();
        public void SaveAll();
    }
}