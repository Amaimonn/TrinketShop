using R3;

namespace TrinketShop.Game.GameData.Upgrades
{
    public class UpgradesModel : Model<UpgradesState>
    {
        public ReactiveProperty<uint> TrinketClickLevel;
        public ReactiveProperty<uint> TrinketDeliveryLevel;

        public UpgradesModel(UpgradesState state) : base(state)
        {
            TrinketClickLevel = new ReactiveProperty<uint>(state.TrinketClickLevel);
            TrinketClickLevel.Skip(1).Subscribe(_ => State.TrinketClickLevel = TrinketClickLevel.Value);

            TrinketDeliveryLevel = new ReactiveProperty<uint>(state.TrinketDeliveryLevel);
            TrinketDeliveryLevel.Skip(1).Subscribe(_ => State.TrinketDeliveryLevel = TrinketDeliveryLevel.Value);
        }
    }
}