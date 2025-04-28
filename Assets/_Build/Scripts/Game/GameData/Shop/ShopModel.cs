using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace TrinketShop.Game.GameData.Shop
{
    public class ShopModel : Model<ShopState>
    {
        public ObservableDictionary<string, int> PurchasedItemsMap;
        
        public ShopModel(ShopState state) : base(state)
        {
            var purchacesMap = new Dictionary<string, int>(State.AllPurchasesInfo.Count);

            foreach (var purchace in State.AllPurchasesInfo)
            {
                purchacesMap.Add(purchace.Id, purchace.Count);
            }

            PurchasedItemsMap = new ObservableDictionary<string, int>(purchacesMap);
            PurchasedItemsMap.ObserveChanged().Subscribe(x => {
                var purchaceIndex = State.AllPurchasesInfo.FindIndex(p => p.Id == x.NewItem.Key);
                State.AllPurchasesInfo[purchaceIndex].Count = x.NewItem.Value;
            });
            PurchasedItemsMap.ObserveAdd().Subscribe(x => {
                State.AllPurchasesInfo.Add(new PurchaseInfo { Id = x.Value.Key, Count = x.Value.Value });
            });
        }
    }
}