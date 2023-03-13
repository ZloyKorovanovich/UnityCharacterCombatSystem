using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    private AttackInputs _inputs;
    private AttackOutputs _outputs;


    private Vector3 _lastPosition;
    private Vector3 _lastEulerAngles;

    private float _velocityMagnitude;
    private float _velocityAngular;

    private void Update()
    {
        _velocityMagnitude = Vector3.SqrMagnitude(transform.position - _lastPosition * Time.deltaTime);
        _velocityAngular = Vector3.Angle(_lastEulerAngles, transform.eulerAngles * Time.deltaTime);
        _lastPosition = transform.position;
    }

    private void OnEnable()
    {
        _lastPosition = transform.position;
        _velocityMagnitude = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.gameObject.layer == _inputs.Weapon.AttackableLayer)
        {
            _inputs.Weapon.DamageCalculator.CalculateDamage(new DamageCalculatorInputs(_inputs.Weapon.Damage, _velocityMagnitude, _velocityAngular), out DamageCalculatorOutputs outputDamage);
            float damage = outputDamage.Damage;
        }
    }

    public void SetStats(AttackInputs inputs)
    {
        _inputs = inputs;
    }
}

public struct AttackInputs
{
    private Weapon _weapon;
    private WeaponController _owner;

    public Weapon Weapon => _weapon;
    public WeaponController Owner => _owner;


    public AttackInputs(Weapon weapon, WeaponController owner)
    {
        _weapon = weapon;
        _owner = owner;
    }
}

public struct AttackOutputs
{
    private float _damage;

    public float Damage => _damage;


    public AttackOutputs(float damage)
    {
        _damage = damage;
    }
}
