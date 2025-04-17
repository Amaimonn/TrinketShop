using TrinketShop.Solutions.UI.MVVM;

namespace TrinketShop.Game.UI.MVVM.GameplayHUD
{
    public class GameplayHudView : ScreenView<GameplayHudViewModel>
    {
        protected override CanvasOrder Order => CanvasOrder.First;

        protected override void OnBind(GameplayHudViewModel viewModel)
        {
            
        }
    }
}