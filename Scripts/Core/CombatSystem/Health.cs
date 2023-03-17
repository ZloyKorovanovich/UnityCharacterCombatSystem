public class Health
{
    private float _healthValue;
    private HealthOutputs _outputs;

    public float HealthValue => _healthValue;


    public Health(float healthValue)
    {
        _healthValue = healthValue;
    }


    public void Change(HealthInputs inputs, out HealthOutputs outputs)
    {
        _healthValue += inputs.Amoumt;
        _outputs = new HealthOutputs(_healthValue <= 0);
        outputs = _outputs;
    }

    public void CheckDeath(out HealthOutputs outputs)
    {
        outputs = _outputs;
    }
}

public struct HealthInputs
{
    private float _amount;

    public float Amoumt => _amount;


    public HealthInputs(float amount)
    {
        _amount = amount;
    }
}

public struct HealthOutputs
{
    private bool _dead;

    public bool Dead => _dead;


    public HealthOutputs(bool dead)
    {
        _dead = dead;
    }
}
