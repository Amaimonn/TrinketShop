namespace TrinketShop.Game.GameData.Entities.Trinket
{
    public class TrinketState : SaveState<TrinketState>
    {
        public uint Id;
        public int Level;
        
        public override TrinketState Copy()
        {
            return (TrinketState)MemberwiseClone();
        }
    }
}