using UnityEngine;
using StarterAssets;
using UnityEngine.Events;

public class PossessionController : MonoBehaviour, IObserver
{
    public UnityEvent CurrentPossessionChanged = new UnityEvent();

    public GameObject CurrentPossession;

    public Throwable currentThrowable;
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
        CurrentPossessionChanged?.Invoke();
        if (CurrentPossession.TryGetComponent(out Throwable throwable))
        {
            currentThrowable = throwable;
        }
    }

    public void Unpossess()
    {
        if (CurrentPossession != null)
        {
            CurrentPossession.GetComponent<IPossessable>().Unpossess();
            CurrentPossession = null;
            currentThrowable = null;
            _thirdPersonController.freeze = false;
            CurrentPossessionChanged?.Invoke();
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
