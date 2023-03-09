using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotEnemyInput : MainInput
{
    private Transform _enemy;

    private MainComponent _characterMainComponent;

    private Vector3 _targetPosition;
    private Vector3 _inputAxis;
    private bool _isAttack;

    // Use this for initialization
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Player").transform;
        _characterMainComponent = GetComponent<CharacterMain>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistance = Vector3.Distance(_enemy.position, transform.position);

        _inputAxis.z = currentDistance - 2f;

        _isAttack = currentDistance < 2.5f;
        _targetPosition = _enemy.position + Vector3.up * 1.5f;

        _characterMainComponent.SetInputs(_inputAxis, _targetPosition, _isAttack);

    }
}
