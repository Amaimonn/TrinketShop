using UnityEngine;

using TrinketShop.Solutions.UI.MVVM;
using TrinketShop.Game.World.GameField;
using TrinketShop.Infrastructure.Providers;
using LostKaiju.Services.Saves;
using TrinketShop.Solutions.Saves;
using TrinketShop.Game.GameData.Map;
using TrinketShop.Game.GameData.Entities.Trinket;
using TrinketShop.Game.Services;
using TrinketShop.Game.World.Trinkets;

namespace TrinketShop.Infrastructure.Bootstrap
{
    public class GameplayBootstrap : MonoBehaviour
    {
        [SerializeField] private RootUIBinder _rootUIBinder;
        [SerializeField] private GameFieldBounds _gameFieldBounds;

        public void Boot ()
        {
            _gameFieldBounds.Init();
            var serializer = new JsonUtilitySerializer();
            var storage = new FileStorage("json");
            var saveSystem = new SimpleSaveSystem(serializer, storage);
            var defaultGameStateProvider = new DefaultGameStateProvider();
            var gameStateProvider = new GameStateProvider(saveSystem, defaultGameStateProvider);

            gameStateProvider.LoadAll();

            var mapModel = new MapModel(gameStateProvider.GameState.MapState);
            var trinketBasePrefab = Resources.Load<TrinketEntity>("TrinketBasePrefab");
            var trinketsConfig = Resources.Load<TrinketConfigSO>("TrinketConfigSO");
            var trinketsService = new TrinketsService(mapModel.Trinkets, trinketsConfig, _gameFieldBounds);
            var pointerService = new WorldInteractionService(trinketsService.TrinketViewModelsMap);
            
            foreach(var trinketViewModelPair in trinketsService.TrinketViewModelsMap)
            {
                var trinketEntity = Instantiate(trinketBasePrefab);
                trinketEntity.Bind(trinketViewModelPair.Value);
            }
            Debug.Log("Gameplay Boot");
        }
    }
}