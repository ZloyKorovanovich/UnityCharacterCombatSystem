using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MainInput
{
    [SerializeField]
    private LayerMask _itemLayer;

    private MainComponent _characterMainComponent;

    private ThirdPersonController _thirdPersonController;

    private Camera _cameraMain;
    private Transform _cameraTransform;

    private Vector3 _targetPosition;
    private Vector3 _inputAxis;
    private bool _isAttack;

    private RaycastHit _hit;

    private void Start()
    {
        _characterMainComponent = GetComponent<CharacterMain>();
        _cameraMain = Camera.main;
        _cameraTransform = _cameraMain.transform;
        _thirdPersonController = new ThirdPersonController(transform);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        _thirdPersonController.CountInputAxis(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), out _inputAxis);

        _isAttack = Input.GetMouseButton(0);

        _thirdPersonController.CountCameraAndTargetMovement(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), ref _cameraTransform, ref _targetPosition, Input.mouseScrollDelta.y * 10f);
        if (Input.GetKeyUp("e"))
        {
            if(Physics.SphereCast(_cameraTransform.position, 2f, _targetPosition, out _hit, 100f, _itemLayer, QueryTriggerInteraction.Ignore))
            {
                ItemOnGround item =_hit.transform.GetComponent<ItemOnGround>();
                if (item)
                {
                    GameObject weapon;
                    item.PickUp(out weapon);
                    _characterMainComponent.SetWeapon(weapon, Input.GetMouseButton(1));
                }
            }
            else
                _characterMainComponent.RemoveWeapon();
        }

        _characterMainComponent.SetInputs(_inputAxis, _targetPosition, _isAttack);
    }
}
