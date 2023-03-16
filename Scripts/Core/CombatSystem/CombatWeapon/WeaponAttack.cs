using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    private static HealthOutputs _DEFAULT_DAMAGE_RESULT = new HealthOutputs(false);
    private static AttackOutput _DEFAULT_ATTACK_BLOCK_OUTPUTS = new AttackOutput(0f, true, _DEFAULT_DAMAGE_RESULT);

    private AttackInputs _inputs;
    private List<AttackOutput> _outputs = new List<AttackOutput>();

    private Vector3 _lastPosition;
    private Vector3 _lastEulerAngles;

    private float _velocityMagnitude;
    private float _velocityAngular;

    private bool _killed;
    private bool _blocked;
    private float _damage;
    private HealthOutputs _attackResult;



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

    private void OnDisable()
    {
        _inputs.Owner.
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.layer == _inputs.Weapon.AttackableLayer)
        {
            _inputs.Weapon.DamageCalculator.CalculateDamage(new DamageCalculatorInputs(_inputs.Weapon.Damage, _velocityMagnitude, _velocityAngular), out DamageCalculatorOutputs outputDamage);
            _damage = outputDamage.Damage;
            CombatController enemyCombatController = other.transform.GetComponent<CombatController>();
            if (!enemyCombatController)
                return;
            enemyCombatController.CharacterHealth.Change(new HealthInputs(-_damage), out _attackResult);
            _outputs.Add(new AttackOutput(_damage, false, _attackResult));
            return;
        }
        _outputs.Add(_DEFAULT_ATTACK_BLOCK_OUTPUTS);
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


public struct AttackOutput
{
    private HealthOutputs _damageResult;

    private float _damage;
    private bool _blocked;

    public HealthOutputs DamageResult => _damageResult;

    public float Damage => _damage;
    public bool Blocked => _blocked;


    public AttackOutput(float damage, bool blocked, HealthOutputs damageResult)
    {
        _damage = damage;
        _blocked = blocked;
        _damageResult = damageResult;
    }
}
