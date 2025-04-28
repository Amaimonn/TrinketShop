using R3;

namespace TrinketShop.Game.GameData.Settings
{
    public class SettingsModel : Model<SettingsState>
    {
        public readonly ReactiveProperty<int> MusicVolume;
        public readonly ReactiveProperty<int> SfxVolume;

        public SettingsModel(SettingsState state) : base(state)
        {
            MusicVolume = new ReactiveProperty<int>(state.MusicVolume);
            MusicVolume.Skip(1).Subscribe(x => state.MusicVolume = x);

            SfxVolume = new ReactiveProperty<int>(state.SfxVolume);
            SfxVolume.Skip(1).Subscribe(x => state.SfxVolume = x);
        }
    }
}