namespace TrinketShop.Solutions.Saves
{
    public interface ISaveSystem
    {
        public void Save<T>(string key, T data);
        public string LoadRaw(string key);
        public void Delete(string key);
        public bool Exists(string key);
    }
}