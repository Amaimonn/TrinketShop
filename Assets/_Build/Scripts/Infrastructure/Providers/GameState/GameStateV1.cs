using TrinketShop.Game.GameData.Currency;
using TrinketShop.Game.GameData.Map;
using TrinketShop.Game.GameData.Shop;
using TrinketShop.Game.GameData.Upgrades;

namespace TrinketShop.Infrastructure.Providers
{
    public class GameStateV1 : BaseGameState, IGameState
    {
        public CurrencyState CurrencyState { get; set; }

        public MapState MapState { get; set; }

        public UpgradesState UpgradesState { get; set; }

        public ShopState ShopState { get; set; }
    }
}