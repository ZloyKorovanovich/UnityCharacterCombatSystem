using UnityEngine;

public class Weapon
{
    private LayerMask _attackableLayer;
    private float _damage;
    private DamageClaculator _damageCalculator;
    

    public LayerMask AttackableLayer => _attackableLayer;
    public float Damage => _damage;
    public DamageClaculator DamageCalculator;
}

public class DamageClaculator
{
    public void CalculateDamage(DamageCalculatorInputs inputs, out DamageCalculatorOutputs outputs)
    {
        outputs = new DamageCalculatorOutputs(inputs.Damage * inputs.VelocityMagnitude * Mathf.Cos(inputs.VelocityAngular));
    }
}

public struct DamageCalculatorInputs
{
    private float _damage;
    private float _velocityMagnitude;
    private float _velocityAngular;

    public float Damage => _damage;
    public float VelocityMagnitude => _velocityMagnitude;
    public float VelocityAngular => _velocityAngular;

    public DamageCalculatorInputs(float damage, float velocityMagnitude, float velocityAngular)
    {
        _damage = damage;
        _velocityMagnitude = velocityMagnitude;
        _velocityAngular = velocityAngular;
    }
}

public struct DamageCalculatorOutputs
{
    private float _damage;

    public float Damage => _damage;


    public DamageCalculatorOutputs(float damage)
    {
        _damage = damage;
    }
}
