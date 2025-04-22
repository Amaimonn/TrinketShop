namespace TrinketShop.Game.GameData.Currency
{
    [System.Serializable]
    public class CurrencyState : ICopyable<CurrencyState>
    {
        public ulong Coins;
        public uint Gems;

        public CurrencyState Copy()
        {
            return (CurrencyState)MemberwiseClone();
        }
    }
}