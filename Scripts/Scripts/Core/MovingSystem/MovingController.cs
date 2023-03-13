using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class MovingController : ControllerAbstract
{
    //StartingInputs
    [SerializeField]
    private float _sensetivity = 7f;
    [SerializeField]
    private float _luft = 60f;
    //LookWeight (weight, bodyWeight, headWeight)
    [SerializeField]
    private Vector3 _animatorIkLookWeight = new Vector3(1f, 0.1f, 0.7f);

    //PhysicalComponents
    private Animator _animator;
    private CharacterController _characterController;

    //ClassesComponents
    private Mover _mover;

    //UpdatingInputs
    private MoverInputs _inputs;

    //UpdatingOutputs
    private MoverOtputs _outputs;


    private Vector3 _lastFramePosition;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _mover = new Mover(_animator, _sensetivity, _luft);

        _lastFramePosition = transform.position;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _characterController.Move(Physics.gravity * Time.deltaTime);
        _mover.Move(_inputs.InputAxis, Time.deltaTime, transform, _inputs.TargetPosition);
        _animator.SetLookAtWeight(_animatorIkLookWeight.x, _animatorIkLookWeight.y, _animatorIkLookWeight.z);
        _animator.SetLookAtPosition(_inputs.TargetPosition);

        CalculateOutputs();
    }


    public override void SetInputs(CharacterInput characterInput)
    {
        _inputs = characterInput.GetMoverInputs();
    }

    public override void SetOutputs(ref CharacterOutput characterOutput)
    {
        characterOutput.SetMoverOutputs(_outputs);
    }


    private void CalculateOutputs()
    {
        _outputs = new MoverOtputs(transform.position, transform.eulerAngles, Vector3.SqrMagnitude(transform.position - _lastFramePosition / Time.deltaTime));
        _lastFramePosition = transform.position;
    }
}