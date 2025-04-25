using UnityEngine;

namespace TrinketShop.Game.GameData.Entities.Trinket
{
    public interface ITrinketLevelConfig 
    {
        public Sprite Sprite { get; }
        public float SizeX { get; }
        public float SizeY { get; }
        public uint BaseIncome { get; }
        public float PassiveIncomeCooldown { get; }
    }
}