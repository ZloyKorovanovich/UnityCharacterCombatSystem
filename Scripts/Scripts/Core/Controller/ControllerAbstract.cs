using UnityEngine;

public abstract class ControllerAbstract : MonoBehaviour
{
    public abstract void SetInputs(CharacterInput characterInput);
    public abstract void SetOutputs(ref CharacterOutput characterOutput);
}
