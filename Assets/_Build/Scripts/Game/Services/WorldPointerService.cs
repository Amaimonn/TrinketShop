using System.Collections.Generic;
using ObservableCollections;
using R3;

using TrinketShop.Game.World.Trinkets;

namespace TrinketShop.Game.Services
{
    public class WorldPointerService
    {
        private readonly IReadOnlyObservableDictionary<uint, TrinketViewModel> _trinketsMap;
        private TrinketViewModel _draggingTrinket;
        private TrinketViewModel _toEnterAfterDrag;
        private TrinketViewModel _toExitAfterDrag;

        public WorldPointerService(IReadOnlyObservableDictionary<uint, TrinketViewModel> trinketsMap)
        {
            _trinketsMap = trinketsMap;
            foreach (var trinketViewModelPair in _trinketsMap)
            {
                ListenTrinket(trinketViewModelPair);
            }

            _trinketsMap.ObserveAdd().Subscribe(x => ListenTrinket(x.Value));
        }

        private void EnterRequest(uint id)
        {
            var trinketToEnter = _trinketsMap[id];
            if (_draggingTrinket != null)
            {
                if (_toExitAfterDrag != null && _toExitAfterDrag == trinketToEnter)
                {
                    _toExitAfterDrag = null;
                    return;
                }
                _toEnterAfterDrag = trinketToEnter;
                return;
            }

            trinketToEnter.IsEntered.Value = true;
        }

        private void ExitRequest(uint id)
        {
            var trinketToExit = _trinketsMap[id];
            if (_draggingTrinket != null)
            {
                if (_toEnterAfterDrag != null && _toEnterAfterDrag == trinketToExit)
                {
                    _toEnterAfterDrag = null;
                    return;
                }
                if (_draggingTrinket == trinketToExit)
                {
                    _toExitAfterDrag = trinketToExit;
                }
                return;
            }

            trinketToExit.IsEntered.Value = false;
        }

        private void BeginDragRequest(uint id)
        {
            if (_draggingTrinket != null)
                return;
            _draggingTrinket = _trinketsMap[id];
            _draggingTrinket.IsDragging.Value = true;
        }

        private void EndDragRequest(uint id)
        {
            if (_draggingTrinket != null)
            {
                _draggingTrinket.IsDragging.Value = false;
                _draggingTrinket = null;
            }
            if (_toEnterAfterDrag != null)
            {
                _toEnterAfterDrag.IsEntered.Value = true;
                _toEnterAfterDrag = null;
            }
            if (_toExitAfterDrag != null)
            {
                _toExitAfterDrag.IsEntered.Value = false;
                _toExitAfterDrag = null;
            }
        }

        private void ListenTrinket(KeyValuePair<uint, TrinketViewModel> trinketViewModelPair)
        {
            trinketViewModelPair.Value.IsEnteredRequest.Subscribe(x => 
            {
                if (x)
                    EnterRequest(trinketViewModelPair.Key);
                else
                    ExitRequest(trinketViewModelPair.Key);
            });

            trinketViewModelPair.Value.IsDraggingRequest.Subscribe(x =>
            {
                if (x)
                    BeginDragRequest(trinketViewModelPair.Key);
                else
                    EndDragRequest(trinketViewModelPair.Key);
            });
    }
    }
}