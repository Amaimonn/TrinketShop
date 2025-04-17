using TrinketShop.Solutions.UI.MVVM;

namespace TrinketShop.Game.UI.MVVM.Footer
{
    public class FooterView : ScreenView<FooterViewModel>
    {
        protected override CanvasOrder Order => CanvasOrder.Last;

        protected override void OnBind(FooterViewModel viewModel)
        {
            
        }
    }
}