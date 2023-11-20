using UnityEngine;
using StarterAssets;
using UnityEngine.Events;

public class PossessionController : MonoBehaviour, IObserver
{
    public UnityEvent CurrentPossessionChanged = new UnityEvent();

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
        if (objectInView == null || objectInView == CurrentPossession)
        {
            return;
        }
        if (!objectInView.TryGetComponent(out IPossessable possessable))
        {
            return;
        }
        if (CurrentPossession != null)
        {
            RemovePossession();
        }
        CurrentPossession = objectInView;
        possessable.Possess();
        CurrentPossession.GetComponent<ObservableObject>().AddObserver(this);
        _thirdPersonController.freeze = true;
        CurrentPossessionChanged?.Invoke();
        if (CurrentPossession.TryGetComponent(out Throwable throwable))
        {
            CurrentThrowable = throwable;
        }
    }

    public void Unpossess()
    {
        if (CurrentPossession != null)
        {
            _thirdPersonController.toUnpossessLocation();
            RemovePossession();
            _thirdPersonController.freeze = false;
            CurrentPossessionChanged?.Invoke();
        }
    }

    private void RemovePossession()
    {
        CurrentPossession.GetComponent<IPossessable>().Unpossess();
        //CurrentPossession.GetComponent<ObservableObject>().RemoveObserver(this);
        CurrentPossession = null;
        CurrentThrowable = null;
    }

    public void OnNotify(ObservableObject observableObject)
    {
        if (observableObject.State == ObjectState.Broken)
        {
            Unpossess();
        }
    }
}
