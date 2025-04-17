using TrinketShop.Game.GameData;

namespace TrinketShop.GameData.Entities.Trinket
{
    public class TrinketState : SaveState<TrinketState>
    {
        public string Id;
        public uint Level;
        

        public override TrinketState Copy()
        {
            return (TrinketState)MemberwiseClone();
        }
    }
}