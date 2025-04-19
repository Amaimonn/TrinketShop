using TrinketShop.Game.Constants;
using TrinketShop.Game.GameData.Currency;
using TrinketShop.Game.GameData.Map;
using TrinketShop.Game.GameData.Upgrades;
using TrinketShop.Solutions.Saves;

namespace TrinketShop.Infrastructure.Providers
{
    public class GameStateProvider : IGameStateProvider
    {
        private ISaveSystem _saveSystem;

        public GameStateProvider(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }

        public CurrencyState CurrencyState { get; private set; }

        public MapState MapState { get; private set; }

        public UpgradesState UpgradesState { get; private set; }

        public ShopState ShopState { get; private set; }

        public void LoadAll()
        {
            if (CurrencyState == null)
                LoadCurrency();
            if (MapState == null)
                LoadMap();
                
            UpgradesState = new UpgradesState();
            ShopState = new ShopState();
        }

        public void SaveAll()
        {

        }
        
        public void LoadCurrency()
        {
            if (_saveSystem.Exists(StateKeys.CURRENCY))
                CurrencyState = _saveSystem.Load<CurrencyState>(StateKeys.CURRENCY);
            else
                CurrencyState = new CurrencyState();
        }

        public void LoadMap()
        {
            if (_saveSystem.Exists(StateKeys.MAP))
                MapState = _saveSystem.Load<MapState>(StateKeys.MAP);
            else
            {
                MapState = new MapState()
                {
                    Trinkets = new()
                    {
                        new(id:1, level: 0),
                        new(id:2, level: 0),
                        new(id:3, level: 0),
                        new(id:4, level: 0),
                        new(id:5, level: 0),
                        new(id:6, level: 0)
                    }
                };
            }
        }

        
    }
}