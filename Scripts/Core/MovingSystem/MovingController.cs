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
    private IKLookWeight _animatorIkLookWeight = new IKLookWeight(1f, 0.7f, 0.9f, 1f, 1f);

    //PhysicalComponents
    private Animator _animator;
    private CharacterController _characterController;

    //ClassesComponents
    private Mover _mover;

    //UpdatingInputs
    private MoverControllerInputs _inputs;

    //UpdatingOutputs
    private MoverControllerOtputs _outputs;


    private Vector3 _lastFramePosition;
    private MoverOutputs _moverOutput;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _mover = new Mover(_animator, _sensetivity, _luft, _animatorIkLookWeight);

        _lastFramePosition = transform.position;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _characterController.Move(Physics.gravity * Time.deltaTime);
        _mover.Move(new MoverInputs(transform, _inputs.InputAxis, _inputs.TargetPosition, Time.deltaTime), out _moverOutput);

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
        _outputs = new MoverControllerOtputs(transform.position, transform.eulerAngles, Vector3.SqrMagnitude(transform.position - _lastFramePosition / Time.deltaTime), _moverOutput);
        _lastFramePosition = transform.position;
    }
}

public struct MoverControllerInputs
{
    private Vector3 _inputAxis;
    private Vector3 _targetPosition;


    public Vector3 InputAxis => _inputAxis;
    public Vector3 TargetPosition => _targetPosition;


    public MoverControllerInputs(Vector3 inputAxis, Vector3 targetPosition)
    {
        _inputAxis = inputAxis;
        _targetPosition = targetPosition;
    }
}

public struct MoverControllerOtputs
{
    private Vector3 _position;
    private Vector3 _eulerAngles;

    private float _movingMagnitude;

    private MoverOutputs _moverOutput;


    public Vector3 Position => _position;
    public Vector3 EulerAngles => _eulerAngles;

    public float MovingMagnitude => _movingMagnitude;

    public MoverOutputs MoverOutput => _moverOutput;


    public MoverControllerOtputs(Vector3 position, Vector3 eulerAngles, float movingMagnitude, MoverOutputs moverOutput)
    {
        _position = position;
        _eulerAngles = eulerAngles;
        _movingMagnitude = movingMagnitude;
        _moverOutput = MoverOutput;
    }
}

public struct IKLookWeight
{
    private float _weight;
    private float _bodyWeight;
    private float _headWeight;
    private float _eyesWeight;
    private float _clampWeight;

    public float Weight => _weight;
    public float BodyWeight => _bodyWeight;
    public float HeadWeight => _headWeight;
    public float EyesWeight => _eyesWeight;
    public float ClampWeight => _clampWeight;


    public IKLookWeight(float weight, float bodyWeight, float headWeight, float eyesWeight, float clampWeight)
    {
        _weight = weight;
        _bodyWeight = bodyWeight;
        _headWeight = headWeight;
        _eyesWeight = eyesWeight;
        _clampWeight = clampWeight;
    }
}
