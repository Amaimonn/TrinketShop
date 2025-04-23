using UnityEngine;
using SystemInfo = UnityEngine.Device.SystemInfo;

namespace TrinketShop.Game.World.Trinkets
{
    public class InteractionService : MonoBehaviour
    {
        private Camera _camera;
        private TrinketEntity _hoveredEntity;
        private TrinketEntity _draggingEntity;
        private int _currentPointerId = -1;
        private Vector2 _lastPointerPosition;
        
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

            if (IsMobile)
            {
                if (isTouchInput)
                {
                    var touch = Input.GetTouch(0);
                    inputPosition = touch.position;
                }
                else
                {
                    return;
                }
            }
            else
            {
                inputPosition = Input.mousePosition;
            }

            Vector3 worldPoint = _camera.ScreenToWorldPoint(inputPosition);
            worldPoint.z = 0;

            // Raycast to find the trinket under the pointer
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            TrinketEntity hitEntity = hit.collider != null ? hit.collider.GetComponent<TrinketEntity>() : null;
            
            // Handle pointer enter/exit
            if (_hoveredEntity != null && _hoveredEntity != hitEntity && _currentPointerId == -1)
            {
                _hoveredEntity.OnPointerExit();
                _hoveredEntity = null;
            }
            
            if (hitEntity != null && _hoveredEntity != hitEntity && _currentPointerId == -1)
            {
                _hoveredEntity = hitEntity;
                _hoveredEntity.OnPointerEnter();
            }

            // Handle pointer down
            if (IsInputDown(out int newPointerId))
            {
                if (_draggingEntity != null)
                    return;
                     
                if (hitEntity != null)
                {
                    _hoveredEntity = hitEntity;
                    _currentPointerId = newPointerId;
                    _lastPointerPosition = inputPosition;
                    _pointerDownTime = Time.time;
                    _pointerDownPosition = inputPosition;
                    _hoveredEntity.OnPointerDown(newPointerId, inputPosition);
                }

                if (!_draggingEntity && _hoveredEntity != null)
                {
                    if (Time.time - _pointerDownTime > DragThresholdTime || Vector2.Distance(_hoveredEntity.transform.position, _pointerDownPosition) > DragThresholdDistance)
                    {
                        StartDrag();
                    }
                }
            }

            // Handle pointer up
            if (IsInputUp(_currentPointerId))
            {
                if (_draggingEntity != null)
                {
                    _draggingEntity.EndDrag();
                    _draggingEntity = null;
                }

                if (_hoveredEntity != null)
                {
                    _hoveredEntity.OnPointerUp(_currentPointerId, inputPosition);

                    if (hitEntity != _hoveredEntity)
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

            if (_hoveredEntity != null && _lastPointerPosition != inputPosition)
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
