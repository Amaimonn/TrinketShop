using DG.Tweening;
using UnityEngine;

namespace TrinketShop.Game.World.Trinkets
{
    public class TrinketEntity : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private TrinketViewModel _viewModel;
        private Sequence _idleSequence;

        public void Bind(TrinketViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void FastClick()
        {
            _viewModel.TriggerClickIncome();
        }

        public void OnPositionUpdated(Vector2 newPosition)
        {
            transform.position = newPosition;
        }

        private void Awake()
        {
            _idleSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .SetRecyclable(true)
                .SetDelay(Random.Range(0, 0.5f));

            _idleSequence.Append(transform.DOScaleY(0.94f, 1f).SetEase(Ease.InOutSine))
                .Append(transform.DOScaleY(1f, 1f).SetEase(Ease.InOutSine))
                .SetLoops(-1);
        }

        private void OnEnable()
        {
            _idleSequence.Play();
        }

        private void OnDisable()
        {
            _idleSequence.Pause();
        }
    }
}