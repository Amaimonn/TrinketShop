using UnityEngine;
using DG.Tweening;
using R3;

using TrinketShop.Game.GameData.Entities.Trinket;

namespace TrinketShop.Game.World.Trinkets
{
    public enum TrinketAnimations
    {
        Idle,
        Hovered,
        Dragged
    }

    public class TrinketEntity : MonoBehaviour, IPointerControllable
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private Transform _visualTransform;
        [SerializeField] private TrinketTweensConfigSO _tweenData;

        private Sequence _idleSequence;
        private Tween _hoverScaleTween;
        private Tween _hoverReturnScaleTween;
        private Tween _hoverExitTween;
        private Tween _returnRotationTween;
        private Tween _clickPunchTween;
        private Tween _dragScaleTween;
        private Tween _dragShakeTween;

        private TrinketViewModel _viewModel;
        private Camera _camera;
        private Vector3 _defaultScale;
        private Quaternion _defaultRotation;

        private bool _isDragging = false;
        private bool _isPointerInside = false;
        private bool _isPointerDown = false;
        private float _pointerDownTime;
        private int _pointerDownId = -1;
        private TrinketAnimations _currentState = TrinketAnimations.Idle;

        public void Bind(TrinketViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.IsDragging.Skip(1).Subscribe(x =>
            {
                if (x)
                    OnBeginDragAccepted();
                else
                    OnEndDragAccepted();
            });

            _viewModel.IsEntered.Skip(1).Subscribe(x =>
            {
                if (x)
                    OnEnterAccepted();
                else
                    OnExitAccepted();
            });

            _viewModel.Position.Subscribe(OnPositionUpdated);
            _viewModel.CurrentLevelConfig.Subscribe(OnCurrentLevelConfigUpdated);
        }

#region IPointerControllable
        public void OnPointerDown(int pointerId, Vector2 position)
        {
            if (!_isPointerDown)
            {
                _pointerDownTime = Time.time;
                _isPointerDown = true;
                _pointerDownId = pointerId;
            }
        }

        public void OnPointerUp(int pointerId, Vector2 position)
        {
            if (_pointerDownId != pointerId)
                return;

            _isPointerDown = false;

            var elapsed = Time.time - _pointerDownTime;
            if (elapsed > 0.5f)
                return;

            _clickPunchTween.Restart();
            _viewModel?.TriggerClickIncome();
        }

        public void OnPointerEnter()
        {
            if (!_isPointerInside)
            {
                _isPointerInside = true;
                _viewModel?.EnterRequest();
            }
        }

        public void OnPointerExit()
        {
            if (_isPointerInside)
            {
                _isPointerInside = false;
                _viewModel?.ExitRequest();
            }
        }

        public void OnPointerMove(Vector2 position)
        {
            if (_isDragging)
                return;

            var worldPos = _camera.ScreenToWorldPoint(position);
            var offset = transform.position - worldPos;
            var tiltX = offset.y * -_tweenData.HoverMaxTilt;
            var tiltY = offset.x * _tweenData.HoverMaxTilt;

            var lerpX = Mathf.LerpAngle(_visualTransform.eulerAngles.x, tiltX, _tweenData.HoverTiltSpeed * Time.deltaTime);
            var lerpY = Mathf.LerpAngle(_visualTransform.eulerAngles.y, tiltY, _tweenData.HoverTiltSpeed * Time.deltaTime);

            _visualTransform.eulerAngles = new Vector3(lerpX, lerpY, 0);
        }

        public void BeginDrag()
        {
            _viewModel?.BeginDragRequest();
        }

        public void EndDrag()
        {
            _viewModel?.EndDragRequest();
        }
#endregion

#region MonoBehaviour
        private void Awake()
        {
            _camera = Camera.main;
            _defaultScale = _visualTransform.localScale;
            _defaultRotation = _visualTransform.rotation;

            CacheIdleAnimation();
            CacheHoverAnimations();
            CacheClickAnimation();
            CacheDragAnimations();
        }

        private void OnEnable()
        {
            _idleSequence.Restart();
        }

        private void OnDisable()
        {
            PauseAnimations();
        }

        private void OnDestroy()
        {
            _idleSequence.Kill();
            _hoverScaleTween.Kill();
            _hoverReturnScaleTween.Kill();
            _hoverExitTween.Kill();
            _returnRotationTween.Kill();
            _clickPunchTween.Kill();
            _dragScaleTween.Kill();
            _dragShakeTween.Kill();
        }

        private void Update()
        {
            if (_isDragging)
            {
                Vector3 targetPos;
                if (Input.touchCount > 0)
                {
                    var touch = GetTouchById(_pointerDownId);
                    if (touch == null) 
                        return;
                    targetPos = _camera.ScreenToWorldPoint(touch.Value.position);
                }
                else
                {
                    targetPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                }

                var newPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _tweenData.DragLerpSpeed);
                newPos.z = -1;
                _viewModel.SetPositionRequest(newPos);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100f);
            }
        }
#endregion

#region Tweens
        private void CacheIdleAnimation()
        {
            _idleSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(_visualTransform.DOScaleY(_defaultScale.y * 0.96f, 1f).SetEase(Ease.InOutSine))
                .Append(_visualTransform.DOScaleY(_defaultScale.y, 1f).SetEase(Ease.InOutSine))
                .SetLoops(-1)
                .SetDelay(Random.Range(0f, 0.5f));
        }

        private void CacheHoverAnimations()
        {
            _hoverScaleTween = _visualTransform.DOScale(_defaultScale * _tweenData.HoverScale, _tweenData.HoverAnimDuration)
                .SetEase(Ease.OutBack)
                .SetAutoKill(false)
                .SetRecyclable(true);

            _hoverReturnScaleTween = _visualTransform.DOScale(Vector3.one, _tweenData.HoverAnimDuration)
                .SetEase(Ease.OutBack)
                .SetAutoKill(false)
                .SetRecyclable(true);

            _hoverExitTween = _visualTransform.DOShakeRotation(_tweenData.HoverAnimDuration, new Vector3(0, 0, _tweenData.HoverShakeStrength),
                _tweenData.HoverShakeVibrato)
                .SetEase(Ease.OutQuad)
                .SetAutoKill(false)
                .SetRecyclable(true);

            _returnRotationTween = _visualTransform.DORotateQuaternion(_defaultRotation, _tweenData.HoverAnimDuration)
                .SetEase(Ease.InOutQuad)
                .SetAutoKill(false)
                .SetRecyclable(true);
        }

        private void CacheClickAnimation()
        {
            _clickPunchTween = _visualTransform.DOPunchRotation(new Vector3(0, 0, _tweenData.ClickPunchStrength),
                _tweenData.ClickPunchDuration, _tweenData.ClickPunchVibrato)
                .SetAutoKill(false)
                .SetRecyclable(true);
        }

        private void CacheDragAnimations()
        {
            _dragScaleTween = _visualTransform.DOScale(_defaultScale * _tweenData.DragScale, _tweenData.HoverAnimDuration)
                .SetEase(Ease.OutBack)
                .SetAutoKill(false)
                .SetRecyclable(true);

            _dragShakeTween = _visualTransform.DOShakeRotation(_tweenData.HoverAnimDuration, new Vector3(0, 0, _tweenData.DragShakeStrength))
                .SetEase(Ease.OutQuad)
                .SetAutoKill(false)
                .SetRecyclable(true);
        }

        private void PauseAnimations()
        {
            _idleSequence.Pause();
            _hoverScaleTween.Pause();
            _hoverReturnScaleTween.Pause();
            _hoverExitTween.Pause();
            _returnRotationTween.Pause();
            _clickPunchTween.Pause();
            _dragScaleTween.Pause();
            _dragShakeTween.Pause();
        }
#endregion

        private void ChangeState(TrinketAnimations newState)
        {
            if (_currentState == newState)
                return;

            switch (_currentState)
            {
                case TrinketAnimations.Idle:
                    _idleSequence.Rewind();
                    break;
                case TrinketAnimations.Hovered:
                    break;
                case TrinketAnimations.Dragged:
                    _dragScaleTween.Rewind();
                    _dragShakeTween.SmoothRewind();
                    break;
            }

            _currentState = newState;

            switch (newState)
            {
                case TrinketAnimations.Idle:
                    _hoverScaleTween.Rewind();
                    _idleSequence.Play();
                    _visualTransform.rotation = _defaultRotation;
                    break;
                case TrinketAnimations.Hovered:
                    _hoverScaleTween.Restart();
                    break;
                case TrinketAnimations.Dragged:
                    _hoverScaleTween.Rewind();
                    _dragScaleTween.Restart();
                    _dragShakeTween.Restart();
                    break;
            }
        }

        private void SetFront(bool isFront)
        {
            var position = transform.position;
            position.z = isFront ? -1 : 0;
            _viewModel.SetPositionRequest(position);
        }

        private Touch? GetTouchById(int touchId)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).fingerId == touchId)
                    return Input.GetTouch(i);
            }
            return null;
        }

#region ViewModel callbacks
        private void OnPositionUpdated(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        private void OnCurrentLevelConfigUpdated(ITrinketLevelConfig currentConfig)
        {
            _spriteRenderer.sprite = currentConfig.Sprite;
            _collider.size = new Vector2(currentConfig.SizeX, currentConfig.SizeY);
        }

        private void OnBeginDragAccepted()
        {
            _isDragging = true;
            SetFront(true);
            ChangeState(TrinketAnimations.Dragged);
        }

        private void OnEndDragAccepted()
        {
            _isDragging = false;
            SetFront(false);
            ChangeState(TrinketAnimations.Idle);
        }

        private void OnEnterAccepted()
        {
            ChangeState(TrinketAnimations.Hovered);
        }

        private void OnExitAccepted()
        {
            ChangeState(TrinketAnimations.Idle);
        }
#endregion
    }
}