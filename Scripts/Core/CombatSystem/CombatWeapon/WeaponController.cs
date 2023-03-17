using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public abstract void SetAttackInputs(WeaponInputs inputs);
}

public struct WeaponInputs
{
    private bool _attack;
    private CombatController _owner;

    public bool Attack => _attack;
    public CombatController Owner => _owner;


    public WeaponInputs(bool attack, CombatController owner)
    {
        _attack = attack;
        _owner = owner;
    }
}

public struct WeaponOutputs
{
    private bool _killed;
    private float _damage;

    public bool Killed => _killed;
    public float Damage => _damage;


    public WeaponOutputs(bool killed, float damage)
    {
        _killed = killed;
        _damage = damage;
    }
}
