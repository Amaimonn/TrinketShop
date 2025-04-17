using UnityEngine;

namespace TrinketShop.Solutions.UI.MVVM
{
    public interface IRootUI
    {
        public void Attach(GameObject gameObjectUI, CanvasOrder order = CanvasOrder.First);

        public void Detach(GameObject gameObjectUI);
    }
}
