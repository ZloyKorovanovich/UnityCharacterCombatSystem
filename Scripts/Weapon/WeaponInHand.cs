using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponInHand : MonoBehaviour
{
    [SerializeField]
    private GameObject _weaponOnGround;

    [SerializeField]
    private float _attackLength;
    [SerializeField]
    private float _attackWidth;
    [SerializeField]
    private float _attackSpeed;
    [SerializeField]
    private float _damage;

    [SerializeField]
    private AnimationCurve _distanceInfluence;
    [SerializeField]
    private LayerMask _attackableLayer;


    public GameObject WeaponOnGround => _weaponOnGround;

    public float AttackLength => _attackLength;
    public float AttackWidth => _attackWidth;
    public float AttackSpeed => _attackSpeed;
    public float Damage => _damage;

    public AnimationCurve DistanceInfluence => _distanceInfluence;
    public LayerMask AttackableLayer => _attackableLayer;
}

