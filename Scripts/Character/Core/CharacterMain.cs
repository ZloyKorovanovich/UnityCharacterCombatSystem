using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterMain : MainComponent, IDamageble
{
    [SerializeField]
    private float _healthPreview = 100f;
    [SerializeField]
    private float _resistancePreview = 1f;


    private const string _ANIMATOR_HIT = "Hit";
    private const string _ANIMATOR_HIT_SPEED = "HitSpeed";


    private Animator _animator;
    private CharacterController _characterController;

    private Vector3 _inputAxis;
    private Vector3 _target;

    private bool _isAttack;

    private Mover _mover;
    private Attacker _attacker;
    private Damager _damager;
    private Health _health;

    private CharacterLife _myLife;

    private WeaponInHand _weapon;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _mover = new Mover(_animator, 7f, 60f);
        _damager = new Damager(_resistancePreview);
        _health = new Health(_healthPreview);
        _myLife = gameObject.AddComponent<CharacterLife>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _characterController.Move(Physics.gravity * Time.deltaTime);
        _mover.Move(_inputAxis, Time.deltaTime, transform, _target);
        if(_weapon)
            _attacker.SetAttack(_isAttack);
        _animator.SetLookAtWeight(1f, 0.1f, 0.7f);
        _animator.SetLookAtPosition(_target);
    }


    public override void SetWeapon(GameObject weapon, bool hand)
    {
        RemoveWeapon();
        Transform bone;
        if (hand)
            bone = _animator.GetBoneTransform(HumanBodyBones.RightHand);
        else
            bone = _animator.GetBoneTransform(HumanBodyBones.LeftHand);
        _weapon = Instantiate(weapon, bone).GetComponent<WeaponInHand>();
        ApplyWeapon();
    }

    public override void RemoveWeapon()
    {
        if (_weapon)
        {
            Instantiate(_weapon.WeaponOnGround, _weapon.transform.position, _weapon.transform.rotation);
            Destroy(_weapon.gameObject);
        }
        _weapon = null;
    }

    private void ApplyWeapon()
    {
        _attacker = new Attacker(_animator, _weapon.AttackLength, _weapon.AttackWidth, _weapon.AttackableLayer, _weapon.AttackSpeed);
    }

    public override void SetInputs(Vector3 inputAxis, Vector3 target, bool isAttack)
    {
        _inputAxis = inputAxis;
        _target = target;
        _isAttack = isAttack;
    }

    public void Attacking()
    {
        if (_attacker != null)
        {
            _attacker.Attack(_weapon.Damage, _weapon.DistanceInfluence, this);
        }
    }

    public void TakeDamage(float damage, float distanceInfluence)
    {
        bool dead = _health.Damage(_damager.CountDamage(damage, distanceInfluence));
        _animator.SetFloat(_ANIMATOR_HIT_SPEED, 1.5f - distanceInfluence);
        _animator.SetTrigger(_ANIMATOR_HIT);
        if (dead)
            Death();
    }

    public void Death()
    {
        RemoveWeapon();
        Destroy(_myLife);
    }
}
