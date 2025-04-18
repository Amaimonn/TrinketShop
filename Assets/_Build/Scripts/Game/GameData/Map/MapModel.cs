using ObservableCollections;
using R3;

using TrinketShop.Game.GameData.Entities.Trinket;

namespace TrinketShop.Game.GameData.Map
{
    public class MapModel : Model<MapState>
    {
        public ObservableDictionary<uint, TrinketModel> Trinkets = new();

        public MapModel(MapState state) : base(state)
        {
            foreach (var trinketState in State.Trinkets)
            {
                var trinketModel = new TrinketModel(trinketState);
                Trinkets.Add(trinketState.Id, trinketModel);
            }
            
            Trinkets.ObserveAdd()
                .Subscribe(x => State.Trinkets.Add(x.Value.Value.State));
            Trinkets.ObserveRemove()
                .Subscribe(x => State.Trinkets.Remove(x.Value.Value.State));
        }
    }
}