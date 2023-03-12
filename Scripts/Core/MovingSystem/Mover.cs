using UnityEngine;

public class Mover
{
    private const string _ANIMATOR_VERTICAL = "Vertical";
    private const string _ANIMATOR_HORIZONTAL = "Horizontal";
    private const string _ANIMATOR_STATE = "State";
    private const string _ANIMATOR_ROTATION = "Rotation";
    private const string _ANIMATOR_IS_ROTATING = "IsRotating";

    private Animator _animator;

    private float _sensetivity;
    private float _luft;

    private bool _isRotating;


    public Mover(Animator animator, float sensetivity, float luft)
    {
        _animator = animator;
        _sensetivity = sensetivity;
        _luft = luft;
    }


    public void Move(Vector3 axis, float deltaTime, Transform body, Vector3 target)
    {
        SetAnimatorAxisStats(axis, deltaTime, _sensetivity);
        SetMovement(body, target, axis, deltaTime, _sensetivity, _luft);
    }

    private void SetAnimatorAxisStats(Vector3 axis, float deltaTime, float sensetivity)
    {
        _animator.SetFloat(_ANIMATOR_VERTICAL, axis.z, 1f / sensetivity, deltaTime);
        _animator.SetFloat(_ANIMATOR_HORIZONTAL, axis.x, 1f / sensetivity, deltaTime);
        _animator.SetFloat(_ANIMATOR_STATE, 1f, 1f / sensetivity, deltaTime);
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
        _animator.SetLookAtWeight(1f, 0.7f, 0.9f, 1f, 1f);
        _animator.SetLookAtPosition(target);
    }

    private void Rotate(Transform body, Vector3 target, bool rotating, float deltaTime, float sensetivity, float luft)
    {
        Vector3 oldRotation = body.eulerAngles;
        body.LookAt(target);

        float angle = Mathf.DeltaAngle(body.eulerAngles.y, oldRotation.y);
        _animator.SetFloat(_ANIMATOR_ROTATION, -Mathf.Sign(angle), 1f / sensetivity, deltaTime);

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

        _animator.SetBool(_ANIMATOR_IS_ROTATING, rotating);
    }
}