using TrinketShop.Game.Constants;
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
                _gameState = _saveSystem.Load<GameStateV1>(StateKeys.GAME_STATE);
                return;
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

        }
    }
}