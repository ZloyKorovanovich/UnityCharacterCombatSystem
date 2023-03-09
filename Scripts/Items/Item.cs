using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemOnGround : MonoBehaviour
{
    public abstract void PickUp(out GameObject item);
}
