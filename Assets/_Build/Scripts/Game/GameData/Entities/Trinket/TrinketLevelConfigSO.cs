using UnityEngine;

namespace TrinketShop.Game.GameData.Entities.Trinket
{
    public class TrinketLevelConfigSO : ITrinketLevelConfig
    {
        [field: SerializeField, Min(0)] public uint Level { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField, Min(0)] public uint BaseIncome { get; private set; }
        [field: SerializeField, Min(1)] public float PassiveIncomeCooldown { get; private set; }
    }
}