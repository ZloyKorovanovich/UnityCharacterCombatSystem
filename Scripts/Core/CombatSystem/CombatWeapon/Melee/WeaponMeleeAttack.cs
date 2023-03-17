using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeleeAttack : MonoBehaviour
{
    private static HealthOutputs _DEFAULT_DAMAGE_RESULT = new HealthOutputs(false);
    private static AttackOutput _DEFAULT_ATTACK_OUTPUTS = new AttackOutput(0f, _DEFAULT_DAMAGE_RESULT);

    private AttackMeleeInputs _inputs;
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
        _inputs.Owner.SetAttackResults(CalculateOutput());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.layer == _inputs.Weapon.AttackableLayer)
        {
            _inputs.Weapon.DamageCalculator.CalculateDamage(new DamageMeleeCalculatorInputs(_inputs.Weapon.Damage, _velocityMagnitude, _velocityAngular), out DamageCalculatorOutputs outputDamage);
            _damage = outputDamage.Damage;
            CombatController enemyCombatController = other.transform.GetComponent<CombatController>();
            if (!enemyCombatController)
                return;
            enemyCombatController.CharacterHealth.Change(new HealthInputs(-_damage), out _attackResult);
            _outputs.Add(new AttackOutput(_damage, _attackResult));
            return;
        }
        StopAttack();
    }

    private AttackOutput CalculateOutput()
    {
        AttackOutput _output = _DEFAULT_ATTACK_OUTPUTS;
        for(int i = 0; i < _outputs.Count; i++)
        {
            _output = new AttackOutput(_outputs[i].Damage, _outputs[i].DamageResult);
        }
        return _output;
    }

    private void StopAttack()
    {

    }

    public void SetStats(AttackMeleeInputs inputs)
    {
        _inputs = inputs;
    }
}

public struct AttackMeleeInputs
{
    private WeaponMelee _weapon;
    private WeaponMeleeController _owner;

    public WeaponMelee Weapon => _weapon;
    public WeaponMeleeController Owner => _owner;


    public AttackMeleeInputs(WeaponMelee weapon, WeaponMeleeController owner)
    {
        _weapon = weapon;
        _owner = owner;
    }
}


public struct AttackOutput
{
    private HealthOutputs _damageResult;

    private float _damage;

    public HealthOutputs DamageResult => _damageResult;

    public float Damage => _damage;


    public AttackOutput(float damage, HealthOutputs damageResult)
    {
        _damage = damage;
        _damageResult = damageResult;
    }
}
