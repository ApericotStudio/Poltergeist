using UnityEngine;
using StarterAssets;
using UnityEngine.Events;

public class PossessionController : MonoBehaviour, IObserver
{
    public UnityEvent<GameObject> OnCurrentPossessionChange = new UnityEvent<GameObject>();

    public GameObject CurrentPossession;

    public Throwable CurrentThrowable;
    private ThirdPersonController _thirdPersonController;
    private VisionController _visionController;

    private void Awake()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _visionController = GetComponent<VisionController>();
    }

    /// <summary>
    /// If possible possess the object the player is looking at
    /// </summary>
    public void Possess()
    {
        GameObject objectInView = _visionController.LookingAt;
        if (objectInView == null)
        {
            return;
        }
        if (!objectInView.TryGetComponent(out IPossessable possessable))
        {
            return;
        }
        if (CurrentPossession != null)
        {
            CurrentPossession.GetComponent<IPossessable>().Unpossess();
        }
        CurrentPossession = objectInView;
        possessable.Possess();
        CurrentPossession.GetComponent<ObservableObject>().AddObserver(this);
        _thirdPersonController.freeze = true;
        OnCurrentPossessionChange?.Invoke(CurrentPossession);
        if (CurrentPossession.TryGetComponent(out Throwable throwable))
        {
            CurrentThrowable = throwable;
        }
    }

    public void Unpossess()
    {
        if (CurrentPossession != null)
        {
            CurrentPossession.GetComponent<IPossessable>().Unpossess();
            CurrentPossession = null;
            CurrentThrowable = null;
            _thirdPersonController.freeze = false;
            OnCurrentPossessionChange?.Invoke(CurrentPossession);
        }
    }

    public void OnNotify(ObservableObject observableObject)
    {
        if (observableObject.State == ObjectState.Broken)
        {
            Unpossess();
        }
    }
}
