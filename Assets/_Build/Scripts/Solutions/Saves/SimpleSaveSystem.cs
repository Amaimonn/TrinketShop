using TrinketShop.Infrastructure.Migrations;

namespace TrinketShop.Solutions.Saves
{
    public class SimpleSaveSystem : ISaveSystem
    {
        private readonly ISerializer _serializer;
        private readonly IDataStorage _storage;

        public SimpleSaveSystem(ISerializer serializer, IDataStorage storage)
        {
            _serializer = serializer;
            _storage = storage;
        }

        public void Save<T>(string key, T data)
        {
            var serializedData = _serializer.Serialize(data);
            _storage.Write(key, serializedData);
        }

        public string LoadRaw(string key)
        {
            var serializedData = _storage.Read(key);

            return serializedData;
        }

        public void Delete(string key)
        {
            _storage.Delete(key);
        }

        public bool Exists(string key)
        {
            var exists = _storage.Exists(key);
            return exists;
        }
    }
}