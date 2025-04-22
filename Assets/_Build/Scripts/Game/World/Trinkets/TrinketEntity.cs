using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Collections;
using DG.Tweening;
using R3;

namespace TrinketShop.Game.World.Trinkets
{
    public enum TrinketAnimations
    {
        Idle,
        Hovered,
        Dragged
    }

    public class TrinketEntity : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler, 
        IPointerUpHandler
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Collider2D _collider;
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
        private Vector2 _pointerDownPosition;
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
        }

        public void FastClick()
        {
            _viewModel.TriggerClickIncome();
        }

        public void OnPositionUpdated(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

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
                Vector3 targetPos = _camera.ScreenToWorldPoint(Input.mousePosition);
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

        private void ChangeState(TrinketAnimations newState)
        {
            if (_currentState == newState) 
                return;

            // Exit current state
            switch (_currentState)
            {
                case TrinketAnimations.Idle:
                    _idleSequence.Rewind();
                    break;
                case TrinketAnimations.Hovered:
                    // _hoverExitTween.Restart();
                    _visualTransform.rotation = _defaultRotation;
                    break;
                case TrinketAnimations.Dragged:
                    _dragScaleTween.Rewind();
                    _dragShakeTween.SmoothRewind();
                    break;
            }

            _currentState = newState;

            // Enter new state
            switch (newState)
            {
                case TrinketAnimations.Idle:
                    _hoverScaleTween.Rewind();
                    _idleSequence.Play();
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

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isPointerDown)
            {
                _pointerDownTime = Time.time;
                _pointerDownPosition = eventData.position;
                _isPointerDown = true;
                _pointerDownId = eventData.pointerId;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_pointerDownId != eventData.pointerId)
                return;

            _isPointerDown = false;
            if (_isDragging)
            {
                EndDrag();
                return;
            }

            if (eventData.dragging) 
                return;

            var elapsed = Time.time - _pointerDownTime;
            if (elapsed > 0.2f) 
                return;
                
            _clickPunchTween.Rewind();
            _clickPunchTween.Play();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isPointerInside)
            {
                _isPointerInside = true;
                _viewModel?.EnterRequest();
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_isDragging && _isPointerDown && _pointerDownId == eventData.pointerId)
            {
                if (Time.time - _pointerDownTime > 0.5f || Vector2.Distance(eventData.position, _pointerDownPosition) > 0.1f)
                    BeginDrag();
            }

            if (_isDragging || eventData.dragging) 
                return;

            Vector3 offset = transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
            float tiltX = offset.y * -_tweenData.HoverMaxTilt;
            float tiltY = offset.x * _tweenData.HoverMaxTilt;

            float lerpX = Mathf.LerpAngle(_visualTransform.eulerAngles.x, tiltX, _tweenData.HoverTiltSpeed * Time.deltaTime);
            float lerpY = Mathf.LerpAngle(_visualTransform.eulerAngles.y, tiltY, _tweenData.HoverTiltSpeed * Time.deltaTime);

            _visualTransform.eulerAngles = new Vector3(lerpX, lerpY, 0);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isPointerInside)
            {
                _isPointerInside = false;
                if (_isPointerDown)
                {
                    _viewModel?.BeginDragRequest();
                }
                _viewModel?.ExitRequest();
            }
        }

        private void BeginDrag()
        {
            _viewModel?.BeginDragRequest();
        }

        private void EndDrag()
        {
            _viewModel?.EndDragRequest();
        }

        private void SetFront(bool isFront)
        {
            var position = transform.position;
            position.z = isFront ? -1 : 0;
            _viewModel.SetPositionRequest(position);
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
            // _isPointerDown = false;
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
    }
}