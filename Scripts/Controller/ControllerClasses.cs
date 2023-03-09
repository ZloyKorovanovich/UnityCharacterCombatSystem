using UnityEngine;

public abstract class Controller
{
    protected Vector3 _cameraOffset;
    protected Transform _bodyTransform;

    public abstract void CountInputAxis(float verticalInput, float horizontalInput, out Vector3 inputAxis);
}

public class TopDownController : Controller
{
    private float _speed;

    public TopDownController(Vector3 cameraOffset, Transform bodyTransform, float speed)
    {
        _cameraOffset = cameraOffset;
        _bodyTransform = bodyTransform;
        _speed = speed;
    }


    public void CountCameraMovement(ref Transform camera)
    {
        camera.position = Vector3.Lerp(camera.position, _bodyTransform.position + _cameraOffset, Time.deltaTime * _speed);

        Vector3 oldRot = camera.eulerAngles;
        camera.LookAt(_bodyTransform);

        oldRot.x = Mathf.LerpAngle(oldRot.x, camera.eulerAngles.x, Time.deltaTime * _speed);
        oldRot.y = 0;
        oldRot.z = 0;

        camera.eulerAngles = oldRot;
    }

    public override void CountInputAxis(float verticalInput, float horizontalInput, out Vector3 inputAxis)
    {
        float angle = _bodyTransform.eulerAngles.y * Mathf.Deg2Rad;

        float cosAngle = Mathf.Cos(angle);
        float sinAngle = Mathf.Sin(angle);

        float vertical = cosAngle * verticalInput + sinAngle * horizontalInput;
        float horizontal = cosAngle * horizontalInput - sinAngle * verticalInput;

        inputAxis = new Vector3(horizontal, 1f, vertical);
    }
}

public class ThirdPersonController : Controller
{
    private float _sensetivity;
    private const float _MIN_BOUND = -10f;
    private const float _MAX_BOUND = 80f;

    private float _zoomSpeed;
    private float _minZoomBound;
    private float _maxZoomBound;
    private float _currentZoom;

    private Vector3 _targetOffset;

    private Vector3 _rotation;

    public ThirdPersonController(Transform bodyTransform)
    {
        _cameraOffset = new Vector3(1f, 1f, -4f);
        _sensetivity = 30f;
        _rotation = Vector3.zero;
        _targetOffset = new Vector3(0, 0, 10);
        _bodyTransform = bodyTransform;
        _zoomSpeed = 5f;
        _minZoomBound = 1f;
        _maxZoomBound = 1.5f;
        _currentZoom = 0.5f * (_maxZoomBound - _minZoomBound) + _minZoomBound;
    }


    public void CountCameraAndTargetMovement(float MouseX, float MouseY, ref Transform camera, ref Vector3 target)
    {
        float deltaTime = Time.deltaTime * _sensetivity;

        _rotation.x -= MouseY * deltaTime;
        _rotation.x = Mathf.Clamp(_rotation.x, _MIN_BOUND, _MAX_BOUND);
        _rotation.y = camera.eulerAngles.y + MouseX * deltaTime;

        camera.eulerAngles = _rotation;
        camera.position = camera.rotation * _cameraOffset + _bodyTransform.position;

        target = camera.transform.position + camera.TransformDirection(_targetOffset);
    }

    public void CountCameraAndTargetMovement(float MouseX, float MouseY, ref Transform camera, ref Vector3 target, float zoom)
    {
        float deltaTime = Time.deltaTime * _sensetivity;

        float zoomValue = zoom + _currentZoom;
        zoomValue = Mathf.Clamp(zoomValue, _minZoomBound, _maxZoomBound);

        _currentZoom = Mathf.Lerp(_currentZoom, zoomValue, Time.deltaTime * _zoomSpeed);

        _rotation.x -= MouseY * deltaTime;
        _rotation.x = Mathf.Clamp(_rotation.x, _MIN_BOUND, _MAX_BOUND);
        _rotation.y = camera.eulerAngles.y + MouseX * deltaTime;

        camera.eulerAngles = _rotation;
        camera.position = camera.rotation * _cameraOffset * _currentZoom + _bodyTransform.position;

        target = camera.transform.position + camera.TransformDirection(_targetOffset);
    }

    public override void CountInputAxis(float verticalInput, float horizontalInput, out Vector3 inputAxis)
    {
        inputAxis = new Vector3(horizontalInput, 1f, verticalInput);
    }
}
