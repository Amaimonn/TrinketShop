using System.Collections.Generic;
using ObservableCollections;
using R3;

using TrinketShop.Game.GameData.Entities.Trinket;
using TrinketShop.Game.World.GameField;
using TrinketShop.Game.World.Trinkets;

namespace TrinketShop.Game.Services
{
    public class TrinketsService
    {
        public IReadOnlyObservableDictionary<uint, TrinketViewModel> TrinketViewModelsMap => _trinketViewModelsMap;

        private readonly ObservableDictionary<uint, TrinketViewModel> _trinketViewModelsMap = new();
        private readonly ITrinketConfig _trinketConfig;
        private readonly IGameField _gameField;

        public TrinketsService(IReadOnlyObservableDictionary<uint, TrinketModel> trinketModelsCollection, 
            ITrinketConfig trinketConfig,
            IGameField gameFieldBounds)
        {
            _trinketConfig = trinketConfig;
            _gameField = gameFieldBounds;

            foreach (var trinketModel in trinketModelsCollection)
            {
                CreateTrinketViewModel(trinketModel);
            }
            trinketModelsCollection.ObserveAdd()
                .Subscribe(x => CreateTrinketViewModel(x.Value));
            trinketModelsCollection.ObserveRemove()
                .Subscribe(x => RemoveTrinketViewModel(x.Value));
        }

        private void CreateTrinketViewModel(KeyValuePair<uint, TrinketModel> trinketModelPair)
        {
            var trinketViewModel = new TrinketViewModel(trinketModelPair.Value, 
                _trinketConfig, 
                _gameField,
                _gameField.GetRandomPosition());
            _trinketViewModelsMap.Add(trinketModelPair.Key, trinketViewModel);
        }

        private void RemoveTrinketViewModel(KeyValuePair<uint, TrinketModel>  trinketModelPair)
        {
            if (_trinketViewModelsMap.TryGetValue(trinketModelPair.Key, out var trinketViewModel))
            {
                trinketViewModel.Dispose();
                _trinketViewModelsMap.Remove(trinketModelPair.Key);
            }
            
        }
    }
}