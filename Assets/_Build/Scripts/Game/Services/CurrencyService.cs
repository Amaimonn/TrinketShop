using TrinketShop.Game.GameData.Currency;

namespace TrinketShop.Game.Services
{
    public class CurrencyService
    {
        private readonly CurrencyModel _currencyModel;

        public CurrencyService(CurrencyModel currencyModel)
        {
            _currencyModel = currencyModel;
        }

        public void AddCoins(ulong amount)
        {
            _currencyModel.Coins.Value += amount;
        }

        public void AddGems(uint amount)
        {
            _currencyModel.Gems.Value += amount;
        }

        public bool TrySpendCoins(ulong amount)
        {
            if (_currencyModel.Coins.Value < amount)
                return false;
                
            _currencyModel.Coins.Value -= amount;
            return true;
        }

        public bool TrySpendGems(uint amount)
        {
            if (_currencyModel.Gems.Value < amount)
                return false;

            _currencyModel.Gems.Value -= amount;
            return true;
        }
    }
}