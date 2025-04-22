using UnityEngine;

using TrinketShop.Game.Constants;

namespace TrinketShop.Infrastructure.Providers
{
    public class DefaultGameStateProvider : IDefaultGameStateProvider
    {
        public void FillWithDefault<T>(ref T gameState) where T : IGameState
        {
            var defaultGameStateSO = Resources.Load<DefaultGameStateSO>(Paths.DEFAULT_GAME_STATE_SO);

            gameState.CurrencyState = defaultGameStateSO.CurrencyState.Copy();
            gameState.MapState = defaultGameStateSO.MapState.Copy();
            gameState.UpgradesState = defaultGameStateSO.UpgradesState.Copy();
            gameState.ShopState = defaultGameStateSO.ShopState.Copy();
        }
    }
}