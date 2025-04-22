using System.Collections.Generic;

using TrinketShop.Game.GameData.Entities.Trinket;

namespace TrinketShop.Game.GameData.Map
{
    [System.Serializable]
    public class MapState : ICopyable<MapState>
    {
        public List<TrinketState> Trinkets = new();

        public MapState Copy()
        {
            var trinketsCopy = new List<TrinketState>(Trinkets.Count);
            foreach (var trinket in Trinkets)
            {
                trinketsCopy.Add(trinket.Copy());
            }

            var stateCopy = new MapState
            {
                Trinkets = trinketsCopy
            };
            
            return stateCopy;
        }
    }
}