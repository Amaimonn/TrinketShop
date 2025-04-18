using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace TrinketShop.Game.World.Trinkets
{
    public class TrinketEntity : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerMoveHandler, IPointerDownHandler, 
        IPointerUpHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Transform _visualTransform;

        [Header("Hover Settings")]
        [SerializeField] private float _hoverScale = 1.1f;
        [SerializeField] private float _hoverShakeStrength = 10f;
        [SerializeField] private int _hoverShakeVibrato = 15;
        [SerializeField] private float _hoverAnimDuration = 0.2f;
        [SerializeField] private float _hoverMaxTilt = 20f;
        [SerializeField] private float _hoverTiltSpeed = 5f;

        [Header("Drag Settings")]
        [SerializeField] private float _dragScale = 1.15f;
        [SerializeField] private float _dragShakeStrength = 10f;
        [SerializeField] private float _dragLerpSpeed = 15f;

        [Header("Click Settings")]
        [SerializeField] private float _clickPunchStrength = 7f;
        [SerializeField] private float _clickPunchDuration = 0.2f;
        [SerializeField] private int _clickPunchVibrato = 20;

        private TrinketViewModel _viewModel;
        
        private Sequence _idleSequence;
        private Tween _hoverScaleTween;
        private Tween _hoverReturnScaleTween;
        private Tween _hoverExitTween;
        private Tween _returnRotationTween;
        private Tween _clickPunchTween;
        private Tween _dragScaleTween;
        private Tween _dragShakeTween;

        private Camera _camera;
        private Vector3 _defaultScale;
        private Quaternion _defaultRotation;
        private bool _isDragging;
        private bool _isIdle = true;
        private bool _isPointerDown = false;
        private bool _isPointerInside = false;
        private float _pointerDownTime;
        private bool _hoverScaleReturned = true;

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
            _camera = Camera.main;
            _defaultScale = _visualTransform.localScale;
            _defaultRotation = _visualTransform.rotation;

            CacheIdleAnimation();
            CacheHoverAnimations();
            CacheClickAnimation();
            CacheDragAnimations();
        }

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
            _hoverScaleTween = _visualTransform.DOScale(_defaultScale * _hoverScale, _hoverAnimDuration)
                .SetEase(Ease.OutBack)
                .SetAutoKill(false)
                .SetRecyclable(true);

            _hoverReturnScaleTween = _visualTransform.DOScale(Vector3.one, _hoverAnimDuration)
                .SetEase(Ease.OutBack)
                .SetAutoKill(false)
                .SetRecyclable(true);

            _hoverExitTween = _visualTransform.DOShakeRotation(_hoverAnimDuration, new Vector3(0, 0, _hoverShakeStrength), 
                _hoverShakeVibrato)
                .SetEase(Ease.OutQuad)
                .SetAutoKill(false)
                .SetRecyclable(true);
            
            _returnRotationTween = _visualTransform.DORotateQuaternion(_defaultRotation, _hoverAnimDuration)
                .SetEase(Ease.InOutQuad)
                .SetAutoKill(false)
                .SetRecyclable(true);
        }

        private void CacheClickAnimation()
        {
            _clickPunchTween = _visualTransform.DOPunchRotation(new Vector3(0, 0, _clickPunchStrength), _clickPunchDuration, _clickPunchVibrato)
                .SetAutoKill(false)
                .SetRecyclable(true);
        }

        private void CacheDragAnimations()
        {
            _dragScaleTween = _visualTransform.DOScale(_defaultScale * _dragScale, _hoverAnimDuration)
                .SetEase(Ease.OutBack)
                .SetAutoKill(false)
                .SetRecyclable(true);

            _dragShakeTween = _visualTransform.DOShakeRotation(_hoverAnimDuration, new Vector3(0, 0, _dragShakeStrength))
                .SetEase(Ease.OutQuad)
                .SetAutoKill(false)
                .SetRecyclable(true);
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

        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDownTime = Time.time;
            _isPointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPointerDown = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerInside = true;
            if (_isDragging || eventData.dragging || eventData.pointerPress) 
                return;

            if (_isIdle)
            {
                _idleSequence.Rewind();
                _isIdle = false;
            }
            _hoverScaleTween.Restart();
            _hoverScaleReturned = false;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_isDragging || eventData.dragging) 
                return;

            Vector3 offset = transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
            float tiltX = offset.y * -_hoverMaxTilt;
            float tiltY = offset.x * _hoverMaxTilt;

            float lerpX = Mathf.LerpAngle(_visualTransform.eulerAngles.x, tiltX, _hoverTiltSpeed * Time.deltaTime);
            float lerpY = Mathf.LerpAngle(_visualTransform.eulerAngles.y, tiltY, _hoverTiltSpeed * Time.deltaTime);

            _visualTransform.eulerAngles = new Vector3(lerpX, lerpY, 0);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerInside = false;
            if (_isDragging || eventData.dragging || _isPointerDown) 
                return;

            if (!_isIdle)
            {
                _idleSequence.Restart();
                _isIdle = true;
            }

            if (!_hoverScaleReturned)
            {
                _hoverReturnScaleTween.Restart();
                _hoverScaleReturned = true;
            }
            _hoverExitTween.Restart();
            _returnRotationTween.Restart();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging) 
                return;

            var elapsed = Time.time - _pointerDownTime;
            if (elapsed > 0.2f) 
                return;
                
            _clickPunchTween.Rewind();
            _clickPunchTween.Play();

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            SetFront(true);
            if (_isIdle)
            {
                _idleSequence.Rewind();
                _isIdle = false;
            }
            _returnRotationTween.Restart();
            _dragScaleTween.Restart();
            _dragShakeTween.Restart();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 targetPos = _camera.ScreenToWorldPoint(eventData.position);
            targetPos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _dragLerpSpeed);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            _isPointerDown = false;
            SetFront(false);
            _dragScaleTween.Rewind();
            _dragShakeTween.SmoothRewind();

            if (!_isPointerInside)
            {
                if (!_hoverScaleReturned)
                {
                    _hoverReturnScaleTween.Restart();
                    _hoverScaleReturned = true;
                }

                if (!_isIdle)
                {
                    _idleSequence.Restart();
                    _isIdle = true;
                }
            }
        }

        private void SetFront(bool isFront)
        {
            var position = transform.position;
            position.z = isFront ? -1 : 0;
            transform.position = position;
        }
    }
}