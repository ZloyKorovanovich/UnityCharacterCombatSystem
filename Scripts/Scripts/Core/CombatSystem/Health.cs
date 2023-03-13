public class Health
{
    private float _healthValue;

    public float HealthValue => _healthValue;


    public Health(float healthValue)
    {
        _healthValue = healthValue;
    }


    public void Change(float amount, out bool dead)
    {
        _healthValue += amount;
        dead = (_healthValue <= 0);
    }
}
