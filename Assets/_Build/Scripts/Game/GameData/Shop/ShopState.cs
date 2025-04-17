using System.Collections.Generic;

namespace TrinketShop.Game.GameData.Map
{
    public class ShopState : SaveState<ShopState>
    {
        public List<string> PurchasedItemsIds = new();
        
        public override ShopState Copy()
        {
            return (ShopState)MemberwiseClone();
        }
    }
}