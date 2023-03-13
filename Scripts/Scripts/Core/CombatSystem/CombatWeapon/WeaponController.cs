using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Weapon _weapon;
    private WeaponAttack _currentAttack;
    private Transform _weaponTransform;

    public void SetAttack()
    {
        if (!_currentAttack)
            _currentAttack = _weaponTransform.gameObject.AddComponent<WeaponAttack>();
        _currentAttack.SetStats(new AttackInputs(_weapon, this));
    }

    public void SetAttackResults(AttackOutputs outputs)
    {

    }
}
