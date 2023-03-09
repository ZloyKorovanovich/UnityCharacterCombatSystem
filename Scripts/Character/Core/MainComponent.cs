using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainComponent : MonoBehaviour
{
    public abstract void SetInputs(Vector3 inputAxis, Vector3 target, bool isAttack);

    public abstract void SetWeapon(GameObject weapon, bool hand);

    public abstract void RemoveWeapon();
}
