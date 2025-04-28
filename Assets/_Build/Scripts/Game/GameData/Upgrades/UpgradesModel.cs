using R3;

namespace TrinketShop.Game.GameData.Upgrades
{
    public class UpgradesModel : Model<UpgradesState>
    {
        public ReactiveProperty<int> BaseClickIncome;
        public ReactiveProperty<int> TrinketDeliverySpeed;
        public ReactiveProperty<int> ClickIncomeCritMultiplier;
        public ReactiveProperty<int> ClickIncomeCritChance;
        public ReactiveProperty<int> PassiveIncomeSpeed;
        public ReactiveProperty<int> PassiveIncomeAmount;
        public ReactiveProperty<int> PassiveIncomeCritMultiplier;
        public ReactiveProperty<int> PassiveIncomeCritChance;

        public UpgradesModel(UpgradesState state) : base(state)
        {
            BaseClickIncome = new ReactiveProperty<int>(state.BaseClickIncome);
            BaseClickIncome.Skip(1).Subscribe(x => State.BaseClickIncome = x);

            TrinketDeliverySpeed = new ReactiveProperty<int>(state.TrinketDeliverySpeed);
            TrinketDeliverySpeed.Skip(1).Subscribe(x => State.TrinketDeliverySpeed = x);
            
            ClickIncomeCritMultiplier = new ReactiveProperty<int>(state.ClickIncomeCritMultiplier);
            ClickIncomeCritMultiplier.Skip(1).Subscribe(x => State.ClickIncomeCritMultiplier = x);
            
            ClickIncomeCritMultiplier = new ReactiveProperty<int>(state.ClickIncomeCritMultiplier);
            ClickIncomeCritMultiplier.Skip(1).Subscribe(x => State.ClickIncomeCritMultiplier = x);
            
            ClickIncomeCritChance = new ReactiveProperty<int>(state.ClickIncomeCritChance);
            ClickIncomeCritChance.Skip(1).Subscribe(x => State.ClickIncomeCritChance = x);
            
            PassiveIncomeSpeed = new ReactiveProperty<int>(state.PassiveIncomeSpeed);
            PassiveIncomeSpeed.Skip(1).Subscribe(x => State.PassiveIncomeSpeed = x);
            
            PassiveIncomeAmount = new ReactiveProperty<int>(state.PassiveIncomeAmount);
            PassiveIncomeAmount.Skip(1).Subscribe(x => State.PassiveIncomeAmount = x);
            
            PassiveIncomeCritChance = new ReactiveProperty<int>(state.PassiveIncomeCritChance);
            PassiveIncomeCritChance.Skip(1).Subscribe(x => State.PassiveIncomeCritChance = x);
            
        }
    }
}