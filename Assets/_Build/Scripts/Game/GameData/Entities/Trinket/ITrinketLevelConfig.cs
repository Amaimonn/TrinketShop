using UnityEngine;

namespace TrinketShop.Game.GameData.Entities.Trinket
{
    public interface ITrinketLevelConfig 
    {
        public uint Level { get; }
        public Sprite Sprite { get; }
        public uint BaseIncome { get; }
        public float PassiveIncomeCooldown { get; }
    }
}