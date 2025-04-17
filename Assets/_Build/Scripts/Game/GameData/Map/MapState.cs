using System.Collections.Generic;

using TrinketShop.Game.GameData.Entities.Trinket;

namespace TrinketShop.Game.GameData.Map
{
    public class MapState : SaveState<MapState>
    {
        public List<TrinketState> Trinkets = new();

        public override MapState Copy()
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