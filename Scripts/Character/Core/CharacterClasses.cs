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

public class Attacker
{
    private const string _ANIMATOR_ATTACK = "IsAttack";
    private const string _ANIMATOR_ATTACK_SPEED = "AttackSpeed";

    private float _attackLength;
    private float _attackWidth;

    private LayerMask _attackable;

    private Animator _animator;
    private Transform _headTransform;


    public Attacker(Animator animator, float attackLength, float attackWidth, LayerMask attackLayer, float attackSpeed)
    {
        _animator = animator;
        _attackLength = attackLength;
        _attackWidth = attackWidth;
        _headTransform = _animator.GetBoneTransform(HumanBodyBones.Head);
        _attackable = attackLayer;
        _animator.SetFloat(_ANIMATOR_ATTACK_SPEED, attackSpeed);
    }

    public void SetAttack(bool isAttack)
    {
        _animator.SetBool(_ANIMATOR_ATTACK, isAttack);
    }

    public void Attack(float damage, AnimationCurve distanceInfluence, IDamageble me)
    {
        Ray ray = new Ray(_headTransform.position, _headTransform.forward);
        RaycastHit[] hited = Physics.SphereCastAll(ray, _attackWidth, _attackLength, _attackable, QueryTriggerInteraction.Ignore);

        for(int i = 0; i < hited.Length; i++)
        {
            IDamageble victim = hited[i].transform.GetComponent<IDamageble>();
            if (victim == null)
                continue;

            if (victim == me)
                continue;

            Animator victimAnimator = hited[i].transform.GetComponent<Animator>();
            if (!victimAnimator)
                continue;
            float normalizedDistance = Vector3.Distance(_headTransform.position, hited[i].point) / _attackLength;
            victim.TakeDamage(damage, distanceInfluence.Evaluate(normalizedDistance));
        }
    }
}

public class Health
{
    private float _health;

    public float GetHealth => _health;


    public Health(float health)
    {
        _health = health;
    }


    public bool Damage(float damage)
    {
        _health -= damage;
        if (_health < 0)
            return true;
        return false;
    }

    public void Heal(float healAmount)
    {
        _health += healAmount;
    }
}

public class Damager
{
    private float _resistion;

    public float Resistion => _resistion;


    public Damager(float resistion)
    {
        _resistion = resistion;
    }


    public float CountDamage(float damage, float distanceInfluence)
    {
        return damage * distanceInfluence * _resistion;
    }
}
