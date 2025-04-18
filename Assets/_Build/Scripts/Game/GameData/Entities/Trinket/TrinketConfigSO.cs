using System.Collections.Generic;
using UnityEngine;

namespace TrinketShop.Game.GameData.Entities.Trinket
{
    [CreateAssetMenu(fileName = "TrinketConfigSO", menuName = "Scriptable Objects/Entities/Trinkets/TrinketConfigSO")]
    public class TrinketConfigSO : ScriptableObject, ITrinketConfig
    {
        [field: SerializeField] public List<ITrinketLevelConfig> LevelConfings { get; private set; }
    }
}