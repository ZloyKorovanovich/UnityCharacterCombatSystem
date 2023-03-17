using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeleeController : WeaponController
{
    private WeaponMelee _weapon;
    private WeaponMeleeAttack _currentAttack;
    private Transform _weaponTransform;

    private AttackOutput _attackResult;

    private WeaponInputs _inputs;


    public override void SetAttackInputs(WeaponInputs inputs)
    {
        _inputs = inputs;
        if(_inputs.Attack)
            SetAttack();
    }

    public void SetAttack()
    {
        if (!_currentAttack)
            _currentAttack = _weaponTransform.gameObject.AddComponent<WeaponMeleeAttack>();
        _currentAttack.SetStats(new AttackMeleeInputs(_weapon, this));
    }

    public void SetAttackResults(AttackOutput outputs)
    {
        _attackResult = outputs;
    }
}
