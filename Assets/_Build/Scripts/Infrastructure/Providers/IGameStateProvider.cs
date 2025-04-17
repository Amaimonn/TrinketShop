namespace TrinketShop.Infrastructure.Providers
{
    public interface IGameStateProvider
    {
        public void LoadAll();
        public void SaveAll();
    }
}