namespace TrinketShop.Game.GameData.Currency
{
    public class CurrencyState : SaveState<CurrencyState>
    {
        public ulong Coins;
        public uint Gems;

        public override CurrencyState Copy()
        {
            return (CurrencyState)MemberwiseClone();
        }
    }
}