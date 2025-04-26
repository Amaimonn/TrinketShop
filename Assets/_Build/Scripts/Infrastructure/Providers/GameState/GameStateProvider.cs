using TrinketShop.Game.Constants;
using TrinketShop.Infrastructure.Migrations;
using TrinketShop.Solutions.Saves;

namespace TrinketShop.Infrastructure.Providers
{
    public class GameStateProvider : IGameStateProvider
    {
        public IGameState GameState => _gameState;

        private readonly ISaveSystem _saveSystem;
        private GameStateV1 _gameState;
        private readonly IDefaultGameStateProvider _defaultGameStateProvider;
        
        public GameStateProvider(ISaveSystem saveSystem, IDefaultGameStateProvider defaultGameStateProvider)
        {
            _saveSystem = saveSystem;
            _defaultGameStateProvider = defaultGameStateProvider;
        }

        public void LoadAll()
        {
            if (_saveSystem.Exists(StateKeys.GAME_STATE))
            {
                var saveFileFata = _saveSystem.LoadRaw(StateKeys.GAME_STATE);
                var migrator = new Migrator();
                if (migrator.TryMigrateIfNecessary(saveFileFata, out _gameState))
                    SaveAll();
            }
            else
            {
                _gameState = new();
                _defaultGameStateProvider.FillWithDefault(ref _gameState);
                SaveAll();
            }
        }

        public void SaveAll()
        {
            _saveSystem.Save(StateKeys.GAME_STATE, _gameState);
        }
    }
}