using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CombatController : MonoBehaviour
{
    private Health _characterHealth;

    public Health CharacterHealth => _characterHealth;

    public void CheckHealth()
    {
        
    }

    public void SetAttackOutputs()
    {

    }
}

public struct CombatControllerInputs
{
    private bool _attack;
}

