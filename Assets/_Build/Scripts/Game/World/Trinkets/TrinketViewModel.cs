using System;
using UnityEngine;
using R3;

using TrinketShop.Game.GameData.Entities.Trinket;
using TrinketShop.Game.Services;

namespace TrinketShop.Game.World.Trinkets
{
    public class TrinketViewModel : IDisposable
    {
        public ReadOnlyReactiveProperty<int> Level => _level;
        public Observable<Unit> OnClickIncomeRequested => _onIncomeRequested;
        public Observable<Unit> OnPassiveIncomeRequested => _onPassiveIncomeRequested;
        public Observable<ITrinketLevelConfig> CurrentLevelConfig => _currentLevelConfig;
        public ReadOnlyReactiveProperty<Vector3> Position => _position;

        public Observable<bool> IsDraggingRequest => _isDraggingRequest;
        public Observable<bool> IsEnteredRequest => _isEnteredRequest;

        public ReactiveProperty<bool> IsDragging = new(false);
        public ReactiveProperty<bool> IsEntered = new(false);

        private readonly ReactiveProperty<int> _level = new();
        private readonly ReactiveProperty<Vector3> _position;
        private readonly ReactiveProperty<ITrinketLevelConfig> _currentLevelConfig = new();
        private readonly Subject<Unit> _onIncomeRequested = new();
        private readonly Subject<Unit> _onPassiveIncomeRequested = new();
        private readonly TrinketModel _model;
        private readonly ReactiveProperty<bool> _isDraggingRequest = new (false);
        private readonly ReactiveProperty<bool> _isEnteredRequest = new (false);

        private readonly ITrinketConfig _config;
        private readonly int _maxLevel;
        private float _cooldown;
        private CompositeDisposable _disposables = new();

        public TrinketViewModel(TrinketModel trinketModel, ITrinketConfig trinketConfig, Vector2 position)
        {
            _model = trinketModel;
            _config = trinketConfig;
            _maxLevel = trinketConfig.LevelConfings.Length - 1;
            _position = new ReactiveProperty<Vector3>(position);

            trinketModel.Level.Subscribe(OnLevelChanged);
        }

        public void SetPositionRequest(Vector3 position)
        {
            _position.Value = position;
        }

        public void BeginDragRequest()
        {
            _isDraggingRequest.Value = true;
        }

        public void EndDragRequest()
        {
            _isDraggingRequest.Value = false;
        }

        public void EnterRequest()
        {
            _isEnteredRequest.Value = true;
        }

        public void ExitRequest()
        {
            _isEnteredRequest.Value = false;
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