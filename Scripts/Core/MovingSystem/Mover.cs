using UnityEngine;

public class Mover
{
    private const string _ANIMATOR_FLOAT_VERTICAL = "Vertical";
    private const string _ANIMATOR_FLOAT_HORIZONTAL = "Horizontal";
    private const string _ANIMATOR_FLOAT_STATE = "State";
    private const string _ANIMATOR_FLOAT_ROTATION = "Rotation";
    private const string _ANIMATOR_BOOL_ROTATING = "Rotating";

    private Animator _animator;

    private IKLookWeight _lookWeight;

    private float _sensetivity;
    private float _luft;

    private bool _isRotating;


    public Mover(Animator animator, float sensetivity, float luft, IKLookWeight lookWeight)
    {
        _animator = animator;
        _sensetivity = sensetivity;
        _luft = luft;
        _lookWeight = lookWeight;
    }

    public void Move(MoverInputs inputs, out MoverOutputs outputs)
    {
        SetAnimatorAxisStats(inputs.Axis, inputs.DeltaTime, _sensetivity);
        SetMovement(inputs.BodyTransform, inputs.Target, inputs.Axis, inputs.DeltaTime, _sensetivity, _luft);
        outputs = new MoverOutputs(_isRotating);
    }

    private void SetAnimatorAxisStats(Vector3 axis, float deltaTime, float sensetivity)
    {
        _animator.SetFloat(_ANIMATOR_FLOAT_VERTICAL, axis.z, 1f / sensetivity, deltaTime);
        _animator.SetFloat(_ANIMATOR_FLOAT_HORIZONTAL, axis.x, 1f / sensetivity, deltaTime);
        _animator.SetFloat(_ANIMATOR_FLOAT_STATE, 1f, 1f / sensetivity, deltaTime);
    }

    private void SetMovement(Transform body, Vector3 target, Vector3 axis, float deltaTime, float sensetivity, float luft)
    {
        SetLook(target);
        bool tempRotating = false;
        if (Mathf.Abs(axis.x) < Mathf.Epsilon && Mathf.Abs(axis.z) < Mathf.Epsilon)
            tempRotating = true;
        Rotate(body, target, tempRotating, deltaTime, sensetivity, luft);
    }

    private void SetLook(Vector3 target)
    {
        _animator.SetLookAtWeight(_lookWeight.Weight, _lookWeight.BodyWeight, _lookWeight.HeadWeight, _lookWeight.EyesWeight, _lookWeight.ClampWeight);
        _animator.SetLookAtPosition(target);
    }

    private void Rotate(Transform body, Vector3 target, bool rotating, float deltaTime, float sensetivity, float luft)
    {
        Vector3 oldRotation = body.eulerAngles;
        body.LookAt(target);

        float angle = Mathf.DeltaAngle(body.eulerAngles.y, oldRotation.y);
        _animator.SetFloat(_ANIMATOR_FLOAT_ROTATION, -Mathf.Sign(angle), 1f / sensetivity, deltaTime);

        angle = Mathf.Abs(angle);
        deltaTime *= sensetivity;

        if (!rotating)
            oldRotation.y = Mathf.LerpAngle(oldRotation.y, body.eulerAngles.y, deltaTime);
        else if (angle > luft)
            _isRotating = true;

        body.eulerAngles = oldRotation;

        if (!_isRotating)
            return;
        else if (angle * Mathf.Deg2Rad <= deltaTime)
        {
            _isRotating = false;
            rotating = false;
        }

        _animator.SetBool(_ANIMATOR_BOOL_ROTATING, rotating);
    }
}

public struct MoverInputs
{
    private Transform _bodyTransform;
    private Vector3 _axis;
    private Vector3 _target;
    private float _deltaTime;

    public Transform BodyTransform => _bodyTransform;
    public Vector3 Axis => _axis;
    public Vector3 Target => _target;
    public float DeltaTime => _deltaTime;


    public MoverInputs(Transform bodyTransform, Vector3 axis, Vector3 target, float deltaTime)
    {
        _bodyTransform = bodyTransform;
        _axis = axis;
        _target = target;
        _deltaTime = deltaTime;
    }
}

public struct MoverOutputs
{
    private bool _rotating;

    public bool Rotating => _rotating;


    public MoverOutputs(bool rotating)
    {
        _rotating = rotating;
    }
}

