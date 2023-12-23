using UnityEngine;

public class ScareCamTrigger : MonoBehaviour
{
    public delegate void ScareCamTriggered(Transform head);
    public event ScareCamTriggered OnScareCamTriggered;

    private void Awake()
    {
        GetComponent<NpcController>().OnStateChange.AddListener(OnStateChanged);
    }

    private void OnStateChanged(IState state)
    {
        if (StateIsStateForScareCam(state))
        {
            OnScareCamTriggered(GetComponent<VisitorSenses>().HeadTransform);
        }
    }

    private bool StateIsStateForScareCam(IState state)
    {
        return state is PanickedState || state is ScaredState;
    }
}
