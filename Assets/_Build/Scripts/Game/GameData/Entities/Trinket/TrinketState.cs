namespace TrinketShop.Game.GameData.Entities.Trinket
{
    public class TrinketState : SaveState<TrinketState>
    {
        public uint Id;
        public int Level;
        
        public TrinketState(uint id, int level = 0)
        {
            Id = id;
            Level = level;
        }
        
        public override TrinketState Copy()
        {
            return (TrinketState)MemberwiseClone();
        }
    }
}