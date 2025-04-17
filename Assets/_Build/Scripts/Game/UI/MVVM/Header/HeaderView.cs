using TrinketShop.Solutions.UI.MVVM;

namespace TrinketShop.Game.UI.MVVM.Header
{
    public class HeaderView : ScreenView<HeaderViewModel>
    {
        protected override CanvasOrder Order => CanvasOrder.Last;

        protected override void OnBind(HeaderViewModel viewModel)
        {
            
        }
    }
}