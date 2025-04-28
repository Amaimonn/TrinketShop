namespace TrinketShop.Game.GameData.Upgrades
{
    [System.Serializable]
    public class UpgradesState : ICopyable<UpgradesState>
    {
        public int BaseClickIncome;
        public int TrinketDeliverySpeed;
        public int ClickIncomeCritMultiplier;
        public int ClickIncomeCritChance;
        public int PassiveIncomeSpeed;
        public int PassiveIncomeAmount;
        public int PassiveIncomeCritMultiplier;
        public int PassiveIncomeCritChance;

        public UpgradesState Copy()
        {
            return (UpgradesState)MemberwiseClone();
        }
    }
}