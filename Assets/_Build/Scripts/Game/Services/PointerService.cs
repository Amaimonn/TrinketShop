using UnityEngine;
using UnityEngine.EventSystems;
using SystemInfo = UnityEngine.Device.SystemInfo;

namespace TrinketShop.Game.World.Trinkets
{
    public class PointerService : MonoBehaviour
    {
#region Debug fields
        [Header("Readonly (debug)")]
        [SerializeField] private TrinketEntity _hoveredEntity;
        [SerializeField] private TrinketEntity _draggingEntity;
        [SerializeField] private int _currentPointerId = -1;
        [SerializeField] private Vector2 _lastPointerPosition;
        [SerializeField] private bool _isPointerDown = false;
        [SerializeField] private bool _isOverUI = false;
#endregion
        private Camera _camera;
        private readonly RaycastHit2D[] _hitCache = new RaycastHit2D[1];
        private float _pointerDownTime;
        private Vector2 _pointerDownPosition;
        private const float DragThresholdDistance = 0.1f;
        private const float DragThresholdTime = 0.5f;
        private static bool IsMobile => SystemInfo.deviceType != DeviceType.Desktop; 

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            bool isTouchInput = Input.touchCount > 0;
            Vector2 inputPosition;
            var wasOverUI = _isOverUI;
            _isOverUI = false;
            if (IsMobile)
            {
                if (isTouchInput)
                {
                    var touch = Input.GetTouch(0);
                    inputPosition = touch.position;
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
                inputPosition = Input.mousePosition;
                _isOverUI = EventSystem.current.IsPointerOverGameObject();
            }

            if (!_isOverUI && wasOverUI) // UI click ended
                return;

            Vector3 worldPoint = _camera.ScreenToWorldPoint(inputPosition);
            worldPoint.z = 0;

            // Raycast to find the trinket under the pointer
            var hitCount = _isOverUI ? 0 : Physics2D.RaycastNonAlloc(worldPoint, Vector2.zero, _hitCache);
            TrinketEntity hitEntity = hitCount > 0 ? _hitCache[0].collider.GetComponent<TrinketEntity>() : null;
            
            // Handle pointer enter/exit
            if (_hoveredEntity != null && _hoveredEntity != hitEntity)
            {
                _hoveredEntity.OnPointerExit();
                _hoveredEntity = null;
            }
            
            if (hitEntity != null && _hoveredEntity != hitEntity)
            {
                _hoveredEntity = hitEntity;
                _hoveredEntity.OnPointerEnter();
            }

            // Handle pointer down
            if (IsInputDown(out int newPointerId))
            {
                _isPointerDown = true;
                if (_draggingEntity != null)
                    return;
                     
                if (hitEntity != null)
                {
                    _hoveredEntity = hitEntity;
                    _currentPointerId = newPointerId;
                    _lastPointerPosition = inputPosition;
                    _pointerDownTime = Time.time;
                    _pointerDownPosition = worldPoint;
                    _hoveredEntity.OnPointerDown(newPointerId, inputPosition);
                }
            }

            // Handle pointer up
            if (IsInputUp(_currentPointerId))
            {
                _isPointerDown = false;
                if (_draggingEntity != null)
                {
                    _draggingEntity.EndDrag();
                    _draggingEntity.OnPointerUp(_currentPointerId, inputPosition);
                    _draggingEntity = null;
                }

                if (_hoveredEntity != null)
                {
                    _hoveredEntity.OnPointerUp(_currentPointerId, inputPosition);
                    if (IsMobile)
                    {
                        _hoveredEntity.OnPointerExit();
                        _hoveredEntity = null;
                    }
                    else if (hitEntity != _hoveredEntity)
                    {
                        _hoveredEntity.OnPointerExit();
                        if (hitEntity != null)
                        {
                            _hoveredEntity = hitEntity;
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

            if (_isPointerDown && !_draggingEntity && _hoveredEntity != null)
            {
                if (Time.time - _pointerDownTime > DragThresholdTime || Vector2.Distance(worldPoint, _pointerDownPosition) > DragThresholdDistance)
                {
                    StartDrag();
                }
            }

            if (_hoveredEntity != null && _lastPointerPosition != inputPosition && !_draggingEntity)
            {
                _hoveredEntity.OnPointerMove(inputPosition);
            }

            _lastPointerPosition = inputPosition;
        }

        private void StartDrag()
        {
            if (_hoveredEntity != null)
            {
                _draggingEntity = _hoveredEntity;
                _draggingEntity.BeginDrag();
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
