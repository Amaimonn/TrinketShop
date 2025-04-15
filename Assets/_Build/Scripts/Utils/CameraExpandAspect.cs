using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraExpandAspect : MonoBehaviour
{
    [SerializeField] private float _targetAspectWidth = 9f;
    [SerializeField] private float _targetAspectHeight = 16f;
    [SerializeField] private float _minOrthoSize = 5f;
    [SerializeField] private float _baseOrthoSize = 5f;

    private Camera _camera;
    private Vector2 _lastScreenSize = new();

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _camera.orthographic = true;
        UpdateCameraSize(new Vector2(Screen.width, Screen.height));
    }

    private void Update()
    {
        var screenSize = new Vector2(Screen.width, Screen.height);
        if (_lastScreenSize != screenSize)
        {
            _lastScreenSize = screenSize;
            UpdateCameraSize(screenSize);
        }
    }

    private void UpdateCameraSize(Vector2 screenSize)
    {
        var targetAspect = _targetAspectWidth / _targetAspectHeight;
        var currentAspect = screenSize.x / screenSize.y;
        
        if (currentAspect > targetAspect) // wide screen, height handling
        {
            _camera.orthographicSize = _baseOrthoSize;
        }
        else // tall screen, width handling
        {
            var scale = targetAspect / currentAspect;
            _camera.orthographicSize = Mathf.Max(_baseOrthoSize * scale, _minOrthoSize);
        }
    }
}