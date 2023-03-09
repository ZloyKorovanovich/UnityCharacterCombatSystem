using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGetter : MonoBehaviour
{
    [SerializeField]
    private GameObject _weapon;
    [SerializeField]
    private bool _hand;

    private void Start()
    {
        if(_weapon)
            GetComponent<MainComponent>().SetWeapon(_weapon, _hand);
        Destroy(this);
    }
}
