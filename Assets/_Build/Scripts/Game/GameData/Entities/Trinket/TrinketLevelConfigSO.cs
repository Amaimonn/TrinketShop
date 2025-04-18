using UnityEngine;

using TrinketShop.Game.World.Trinkets;

namespace TrinketShop.Game.GameData.Entities.Trinket
{
    [CreateAssetMenu(fileName = "TrinketLevelConfigSO", menuName = "Scriptable Objects/Entities/Trinkets/TrinketLevelConfigSO")]
    public class TrinketLevelConfigSO : ScriptableObject, ITrinketLevelConfig
    {
        [field: SerializeField, Min(0)] public uint Level { get; private set; }
        [field: SerializeField] public TrinketEntity Prefab { get; private set; }
        [field: SerializeField, Min(0)] public uint BaseIncome { get; private set; }
        [field: SerializeField, Min(1)] public float PassiveIncomeCooldown { get; private set; }
    }
}