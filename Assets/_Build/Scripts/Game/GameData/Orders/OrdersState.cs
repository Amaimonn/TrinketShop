using System;
using System.Collections.Generic;

namespace TrinketShop.Game.GameData.Orders
{
    [Serializable]
    public class OrdersState : ICopyable<OrdersState>
    {
        public List<OrderState> CurrentOrders;

        public OrdersState Copy()
        {
            var ordersList = new List<OrderState>(CurrentOrders.Count);
            foreach (var order in CurrentOrders)
            {
                ordersList.Add(order.Copy());
            }
            
            return new OrdersState()
            {
                CurrentOrders = ordersList
            };
        }
    }

    [Serializable]
    public class OrderState : ICopyable<OrderState>
    {
        public RequiredItemInfo[] RequiredItems;
        public uint CoinsReward;

        public OrderState Copy()
        {
            var requiredCount = RequiredItems.Length;
            var requiredItemsCopy = new RequiredItemInfo[requiredCount];
            for (var i = 0; i < requiredCount; i++)
            {
                requiredItemsCopy[i] = new RequiredItemInfo()
                {
                    Id = RequiredItems[i].Id,
                    Amount = RequiredItems[i].Amount
                };
            }

            return new OrderState()
            {
                RequiredItems = requiredItemsCopy,
                CoinsReward = CoinsReward
            };
        }
    }

    public struct RequiredItemInfo
    {
        public string Id;
        public int Amount;
    }
}