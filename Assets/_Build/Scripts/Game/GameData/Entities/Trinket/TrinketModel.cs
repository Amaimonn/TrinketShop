using R3;

namespace TrinketShop.Game.GameData.Entities.Trinket
{
    public class TrinketModel : Model<TrinketState>
    {
        public uint Id => State.Id;
        public readonly ReactiveProperty<int> Level;

        public TrinketModel(TrinketState state) : base(state)
        {
            Level = new ReactiveProperty<int>(State.Level);
            Level.Skip(1).Subscribe(x => State.Level = x);
        }
    }
}