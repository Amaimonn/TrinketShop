namespace TrinketShop.Infrastructure.Providers
{
    public interface IDefaultGameStateProvider
    {
        public void FillWithDefault<T>(ref T gameState) where T : IGameState;
    }
}