using System.Collections.Generic;

namespace TrinketShop.Game.GameData.Shop
{
    [System.Serializable]
    public class ShopState : ICopyable<ShopState>
    {
        public List<string> PurchasedItemsIds = new();
        
        public ShopState Copy()
        {
            return (ShopState)MemberwiseClone();
        }
    }
}