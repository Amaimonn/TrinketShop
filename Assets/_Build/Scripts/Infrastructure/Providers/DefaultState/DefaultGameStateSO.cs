using UnityEngine;

using TrinketShop.Game.GameData.Currency;
using TrinketShop.Game.GameData.Map;
using TrinketShop.Game.GameData.Shop;
using TrinketShop.Game.GameData.Upgrades;

namespace TrinketShop.Infrastructure.Providers
{
    [CreateAssetMenu(fileName = "DefaultGameStateSO", menuName = "Scriptable Objects/DefaultGameStateSO")]
    public class DefaultGameStateSO : ScriptableObject, IGameState
    {
        [field: SerializeField] public CurrencyState CurrencyState { get; set; }

        [field: SerializeField] public MapState MapState { get; set; }

        [field: SerializeField] public UpgradesState UpgradesState { get; set; }

        [field: SerializeField] public ShopState ShopState { get; set; }
    }
}