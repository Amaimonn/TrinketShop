namespace TrinketShop.Game.GameData.Settings
{
    public class SettingsState  : ICopyable<SettingsState>
    {
        public int MusicVolume;
        public int SfxVolume;

        public SettingsState Copy()
        {
            return (SettingsState)MemberwiseClone();
        }
    }
}