using UnityEngine;

namespace TrinketShop.Game.GameData.Entities.Trinket
{
    [CreateAssetMenu(fileName = "TrinketConfigSO", menuName = "Scriptable Objects/Entities/Trinkets/TrinketConfigSO")]
    public class TrinketConfigSO : ScriptableObject, ITrinketConfig
    {
        public ITrinketLevelConfig[] LevelConfings => _levelConfingsSO;
        [SerializeField] private TrinketLevelConfigSO[] _levelConfingsSO;
    }
}