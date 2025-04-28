using System;
using System.Collections.Generic;

namespace TrinketShop.Game.GameData.Shop
{
    [Serializable]
    public class ShopState : ICopyable<ShopState>
    {
        public List<PurchaseInfo> AllPurchasesInfo = new();
        
        public ShopState Copy()
        {
            return (ShopState)MemberwiseClone();
        }
    }

    [Serializable]
    public class PurchaseInfo
    {
        public string Id;
        public int Count;
    }
}