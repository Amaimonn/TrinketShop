using UnityEngine;

namespace TrinketShop.Game.GameData.Entities.Trinket
{
    [CreateAssetMenu(fileName = "TrinketLevelConfigSO", menuName = "Scriptable Objects/Entities/Trinkets/TrinketLevelConfigSO")]
    public class TrinketLevelConfigSO : ScriptableObject, ITrinketLevelConfig
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField, Min(0)] public float SizeX { get; private set; }
        [field: SerializeField, Min(0)] public float SizeY { get; private set; }
        [field: SerializeField, Min(0)] public uint BaseIncome { get; private set; }
        [field: SerializeField, Min(1)] public float PassiveIncomeCooldown { get; private set; }
    }
}