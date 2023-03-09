using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOnGround : ItemOnGround
{
    [SerializeField]
    private GameObject _weaponInHand;

    public override void PickUp(out GameObject item)
    {
        item = _weaponInHand;
        Destroy(this.gameObject);
    }
}
