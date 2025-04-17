namespace TrinketShop.Game.GameData.Upgrades
{
    public class UpgradesState : SaveState<UpgradesState>
    {
        public uint TrinketClickLevel;
        public uint TrinketDeliveryLevel;

        public override UpgradesState Copy()
        {
            return (UpgradesState)MemberwiseClone();
        }
    }
}