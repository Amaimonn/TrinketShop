using System.Collections.Generic;

namespace TrinketShop.Game.GameData.Entities.Trinket
{
    public interface ITrinketConfig 
    {
        public ITrinketLevelConfig[] LevelConfings { get; }
    }
}