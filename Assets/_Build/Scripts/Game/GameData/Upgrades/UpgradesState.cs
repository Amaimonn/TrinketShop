namespace TrinketShop.Game.GameData.Upgrades
{
    [System.Serializable]
    public class UpgradesState : ICopyable<UpgradesState>
    {
        public uint TrinketClickLevel;
        public uint TrinketDeliveryLevel;

        public UpgradesState Copy()
        {
            return (UpgradesState)MemberwiseClone();
        }
    }
}