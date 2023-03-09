using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLife : MonoBehaviour
{
    public void OnEnable()
    {
        ActivateRD(transform, true);
    }

    public void OnDisable()
    {
        Die(transform);
    }

    public void Die(Transform root)
    {
        Decompose(root, 0f);
        ActivateRD(root, false);
    }

    private void Decompose(Transform root, float animatorDecompositionTimer)
    {
        MainInput input = root.GetComponent<MainInput>();
        Destroy(input);

        MainComponent main = root.GetComponent<MainComponent>();
        Destroy(main);

        Animator animator = root.GetComponent<Animator>();
        Destroy(animator, animatorDecompositionTimer);

        CharacterController controller = root.GetComponent<CharacterController>();
        Destroy(controller);
    }

    public void ActivateRD(Transform root, bool state)
    {
        Rigidbody[] rbs = root.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rbs.Length; i++)
            rbs[i].isKinematic = state;
    }
}
