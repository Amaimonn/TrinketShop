namespace TrinketShop.Game.GameData.Entities.Trinket
{
    [System.Serializable]
    public class TrinketState : ICopyable<TrinketState>
    {
        public uint Id;
        public int Level;
        
        public TrinketState(uint id, int level = 0)
        {
            Id = id;
            Level = level;
        }
        
        public TrinketState Copy()
        {
            return (TrinketState)MemberwiseClone();
        }
    }
}