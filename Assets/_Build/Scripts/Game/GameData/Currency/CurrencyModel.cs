using R3;

namespace TrinketShop.Game.GameData.Currency
{
    public class CurrencyModel : Model<CurrencyState>
    {
        public ReactiveProperty<ulong> Coins;
        public ReactiveProperty<uint> Gems;

        public CurrencyModel(CurrencyState state) : base(state)
        {
            Coins = new ReactiveProperty<ulong>(state.Coins);
            Coins.Skip(1).Subscribe(x => State.Coins = x);

            Gems = new ReactiveProperty<uint>(state.Gems);
            Gems.Skip(1).Subscribe(x => State.Gems = x);
        }
    }
}