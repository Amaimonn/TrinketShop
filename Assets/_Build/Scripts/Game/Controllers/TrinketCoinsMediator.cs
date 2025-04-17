using UnityEngine;

namespace TrinketShop.Game.Controllers
{
    public class TrinketCoinsMediator
    {
        private uint _additionalBaseCoins;
        private float _baseCoinsMultiplier = 1f;
        
        private uint _additionalCoins;
        private float _coinsMultiplier = 1f;
        
        private uint _clickBonus;
        private float _clickMultiplier = 1f;

        public uint BaseCoins => _additionalBaseCoins;
        public float BaseMultiplier => _baseCoinsMultiplier;
        public uint AdditionalCoins => _additionalCoins;
        public float CoinsMultiplier => _coinsMultiplier;
        public uint ClickBonus => _clickBonus;
        public float ClickMultiplier => _clickMultiplier;

        public void AddBaseCoins(uint amount) => _additionalBaseCoins += amount;
        public void AddBaseMultiplier(float multiplier) => _baseCoinsMultiplier += multiplier;
        
        public void AddAdditionalCoins(uint amount) => _additionalCoins += amount;
        public void AddCoinsMultiplier(float multiplier) => _coinsMultiplier += multiplier;
        
        public void AddClickBonus(uint amount) => _clickBonus += amount;
        public void AddClickMultiplier(float multiplier) => _clickMultiplier += multiplier;

        public void ResetClickBonuses()
        {
            _clickBonus = 0;
            _clickMultiplier = 1f;
        }

        public uint Calculate(uint baseAmount)
        {
            uint modifiedAmount = (uint)Mathf.RoundToInt(
                (baseAmount + _additionalBaseCoins) * _baseCoinsMultiplier);
            
            modifiedAmount = (uint)Mathf.RoundToInt(
                (modifiedAmount + _additionalCoins) * _coinsMultiplier);
            
            modifiedAmount = (uint)Mathf.RoundToInt(
                (modifiedAmount + _clickBonus) * _clickMultiplier);
            
            return modifiedAmount;
        }

        public void ResetAll()
        {
            _additionalBaseCoins = 0;
            _baseCoinsMultiplier = 1f;
            _additionalCoins = 0;
            _coinsMultiplier = 1f;
            ResetClickBonuses();
        }
    }
}