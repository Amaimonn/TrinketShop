using UnityEngine;
using UnityEngine.EventSystems;
using SystemInfo = UnityEngine.Device.SystemInfo;

namespace TrinketShop.Game.World.Trinkets
{
    public class PointerService : MonoBehaviour
    {
#region Debug fields
        [Header("Readonly (debug)")]
        [SerializeField] private int _currentPointerId = -1;
        [SerializeField] private Vector2 _lastPointerPosition;
        [SerializeField] private bool _isPointerDown = false;
        [SerializeField] private bool _isOverUI = false;
#endregion

        private Camera _camera;

        private static bool IsMobile => SystemInfo.deviceType != DeviceType.Desktop; // TODO: YG: Use SDK instead (in Awake)
        private const float DRAG_THRESHOLD_DISTANCE = 0.1f;
        private const float DRAG_THRESHOLD_TIME = 0.5f;

        private IPointerControllable _hoveredEntity;
        private IPointerControllable _draggingEntity;
        private readonly RaycastHit2D[] _hitCache = new RaycastHit2D[1];
        private float _pointerDownTime;
        private Vector2 _pointerDownWorldPosition;
        private IPointerControllable _readHitEntity;
        private Vector2 _readPointerPosition;
        private GameObject _lastHitGameObject;

        public void Refresh()
        {
            _currentPointerId = -1;
            _isPointerDown = false;
            if (_hoveredEntity != null)
            {
                _hoveredEntity.OnPointerExit();
                _hoveredEntity = null;
            }

            if (_draggingEntity != null)
            {
                _draggingEntity.EndDrag();
                _draggingEntity = null;
            }
        }

#region MonoBehaviour
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            _readPointerPosition = default;
            var wasOverUI = _isOverUI;
            _isOverUI = false;

            ReadPointerInput();

            if (!_isOverUI && wasOverUI) // UI click ended
                return;

            Vector3 worldPointerPosition = _camera.ScreenToWorldPoint(_readPointerPosition);
            worldPointerPosition.z = 0;

            // Raycast to find the trinket under the pointer
            var hitCount = _isOverUI ? 0 : Physics2D.RaycastNonAlloc(worldPointerPosition, Vector2.zero, _hitCache);
            if (hitCount > 0)
            {
                var hitGameObject = _hitCache[0].collider.gameObject;
                if (_lastHitGameObject == null || _lastHitGameObject != hitGameObject) // another entity was hit
                {
                    _readHitEntity = hitGameObject.GetComponent<IPointerControllable>();
                    _lastHitGameObject = hitGameObject;
                }
            }
            else // no entity was hit
            {
                _readHitEntity = null;
                _lastHitGameObject = null;
            }
            
            HandlePointerExit();
            
            HandlePointerEnter();

            HandlePointerDown(worldPointerPosition);

            HandlePointerUp();

            HandleStartDrag(worldPointerPosition);

            HandlePointerMove();

            _lastPointerPosition = _readPointerPosition;
        }
# endregion

        private void ReadPointerInput()
        {
            bool isTouchInput = Input.touchCount > 0;
            if (IsMobile)
            {
                if (isTouchInput)
                {
                    var touch = Input.GetTouch(0);
                    _readPointerPosition = touch.position;
                    _isOverUI = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
                }
                else
                {
                    if (_draggingEntity != null)
                    {
                        _draggingEntity.EndDrag();
                        _draggingEntity = null;
                    }
                    if (_hoveredEntity != null)
                    {
                        _hoveredEntity.OnPointerExit();
                        _hoveredEntity = null;
                    }
                    return;
                }
            }
            else
            {
                _readPointerPosition = Input.mousePosition;
                _isOverUI = EventSystem.current.IsPointerOverGameObject();
            }
        }

        private void HandlePointerExit()
        {
            if (_hoveredEntity != null && _hoveredEntity != _readHitEntity)
            {
                _hoveredEntity.OnPointerExit();
                _hoveredEntity = null;
            }
        }

        private void HandlePointerEnter()
        {
            if (_readHitEntity != null && _hoveredEntity != _readHitEntity)
            {
                _hoveredEntity = _readHitEntity;
                _hoveredEntity.OnPointerEnter();
            }
        }

        private void HandlePointerDown(Vector2 worldPoint)
        {
            if (_draggingEntity != null)
                return;
                
            if (IsInputDown(out int newPointerId))
            {
                _isPointerDown = true;
                _currentPointerId = newPointerId;
                _pointerDownTime = Time.time;
                _lastPointerPosition = _readPointerPosition;
                _pointerDownWorldPosition = worldPoint;
                     
                if (_readHitEntity != null)
                {
                    _hoveredEntity = _readHitEntity;
                    _hoveredEntity.OnPointerDown(newPointerId, _readPointerPosition);
                }
            }
        }

        private void HandlePointerUp()
        {
            if (IsInputUp(_currentPointerId))
            {
                _isPointerDown = false;
                HandleEndDrag();

                if (_hoveredEntity != null)
                {
                    _hoveredEntity.OnPointerUp(_currentPointerId, _readPointerPosition);
                    if (IsMobile)
                    {
                        _hoveredEntity.OnPointerExit();
                        _hoveredEntity = null;
                    }
                    else if (_readHitEntity != _hoveredEntity)
                    {
                        _hoveredEntity.OnPointerExit();
                        if (_readHitEntity != null)
                        {
                            _hoveredEntity = _readHitEntity;
                            _hoveredEntity.OnPointerEnter();
                        }
                        else
                        {
                            _hoveredEntity = null;
                        }
                    }
                }
                _currentPointerId = -1;
            }
        }

        private void HandleStartDrag(Vector2 worldPoint)
        {
            if (_isPointerDown && _draggingEntity == null && _hoveredEntity != null)
            {
                if (Time.time - _pointerDownTime > DRAG_THRESHOLD_TIME || Vector2.Distance(worldPoint, _pointerDownWorldPosition) > DRAG_THRESHOLD_DISTANCE)
                {
                    _draggingEntity = _hoveredEntity;
                    _draggingEntity.BeginDrag();
                }
            }
        }

        private void HandleEndDrag()
        {
            if (_draggingEntity != null)
            {
                _draggingEntity.EndDrag();
                _draggingEntity.OnPointerUp(_currentPointerId, _readPointerPosition);
                _draggingEntity = null;
            }
        }

        private void HandlePointerMove()
        {
            if (_hoveredEntity != null && _lastPointerPosition != _readPointerPosition)
            {
                _hoveredEntity.OnPointerMove(_readPointerPosition);
            }
        }

        private bool IsInputDown(out int pointerId)
        {
            if (IsMobile)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    pointerId = Input.GetTouch(0).fingerId;
                    return true;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                pointerId = -1;
                return true;
            }
            pointerId = -1;
            return false;
        }

        private bool IsInputUp(int currentPointerId)
        {
            if (IsMobile)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    var touch = Input.GetTouch(i);
                    if (touch.fingerId == currentPointerId && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                        return true;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                return true;
            }
            return false;
        }
    }
}
