using TrinketShop.Solutions.UI.MVVM;

namespace TrinketShop.Game.UI.MVVM.Footer
{
    public class ShopView : ScreenView<FooterViewModel>
    {
        protected override CanvasOrder Order => CanvasOrder.First;

        protected override void OnBind(FooterViewModel viewModel)
        {
            
        }
    }
}