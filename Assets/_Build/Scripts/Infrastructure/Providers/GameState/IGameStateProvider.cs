namespace TrinketShop.Infrastructure.Providers
{
    public interface IGameStateProvider
    {
        public IGameState GameState { get; }

        public void LoadAll();
        public void SaveAll();
    }
}