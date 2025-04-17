using System;
using UnityEngine;
using R3;

using TrinketShop.Game.GameData.Entities.Trinket;

namespace TrinketShop.Game.World.Trinkets
{
    public class TrinketViewModel : IDisposable
    {
        public Observable<int> Level => _level;
        public Observable<Unit> OnClickIncomeRequested => _onIncomeRequested;
        public Observable<Unit> OnPassiveIncomeRequested => _onPassiveIncomeRequested;
        public Observable<ITrinketLevelConfig> CurrentLevelConfig => _currentLevelConfig;
        public ReadOnlyReactiveProperty<Vector2> Position => _position;

        private readonly ReactiveProperty<int> _level = new();
        private readonly ReactiveProperty<Vector2> _position;
        private readonly ReactiveProperty<ITrinketLevelConfig> _currentLevelConfig = new();
        private readonly Subject<Unit> _onIncomeRequested = new();
        private readonly Subject<Unit> _onPassiveIncomeRequested = new();
        private readonly TrinketModel _model;
        private readonly ITrinketConfig _config;
        private readonly int _maxLevel;
        private float _cooldown;
        private CompositeDisposable _disposables = new();

        public TrinketViewModel(TrinketModel trinketModel, ITrinketConfig trinketConfig, Vector2 position)
        {
            _model = trinketModel;
            _config = trinketConfig;
            _maxLevel = trinketConfig.LevelConfings.Count - 1;
            _position = new ReactiveProperty<Vector2>(position);

            trinketModel.Level.Subscribe(OnLevelChanged);
        }

        public void StartWorking()
        {
            Observable.Interval(TimeSpan.FromSeconds(_cooldown))
                .Subscribe(_ => TriggerPassiveIncome())
                .AddTo(_disposables);
        }

        public void TriggerClickIncome()
        {
            _onIncomeRequested.OnNext(Unit.Default);
        }

        public void TriggerPassiveIncome()
        {
            _onPassiveIncomeRequested.OnNext(Unit.Default);
        }

        public void LevelUp()
        {
            if (_level.Value < _maxLevel)
            {
                OnLevelChanged(_level.Value + 1);
            }
        }

        private void OnLevelChanged(int level)
        {
            _level.Value = level;
            var levelConfig = _config.LevelConfings[level];
            _currentLevelConfig.Value = levelConfig;
            _cooldown = levelConfig.PassiveIncomeCooldown;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}