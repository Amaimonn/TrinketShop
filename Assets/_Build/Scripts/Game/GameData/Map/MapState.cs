using System.Collections.Generic;

using TrinketShop.Game.GameData;
using TrinketShop.GameData.Entities.Trinket;

namespace TrinketShop.GameData.Map
{
    public class MapState : SaveState<MapState>
    {
        public List<TrinketState> Trinkets;

        public override MapState Copy()
        {
            throw new System.NotImplementedException();
        }
    }
}