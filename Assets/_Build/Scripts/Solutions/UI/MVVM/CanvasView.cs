namespace TrinketShop.Solutions.UI.MVVM
{
    public abstract class CanvasView<T> : View<T> where T : IViewModel
    {
        protected abstract CanvasOrder Order { get; }

        public sealed override void Attach(IRootUI rootUI)
        {
            rootUI.Attach(gameObject, Order);
        }

        public sealed override void Detach(IRootUI rootUI)
        {
            Dispose();
            rootUI.Detach(gameObject);
        }
    }
}