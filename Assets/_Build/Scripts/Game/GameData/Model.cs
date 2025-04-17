namespace TrinketShop.Game.GameData
{
    public abstract class Model<T> : IModel
    {
        public readonly T State;

        public Model(T state)
        {
            State = state;
        }
    }
}