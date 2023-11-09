using UnityEngine;
using StarterAssets;

public class PossessionController : MonoBehaviour, IObserver
{
    public GameObject CurrentPossession;

    public Throwable currentThrowable;
    private ThirdPersonController _thirdPersonController;
    private VisionController _visionController;
    private AimMode _aimMode;

    private void Awake()
    {
        _aimMode = GetComponent<AimMode>();
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

        _aimMode.changeCameraToPossession();
        if (CurrentPossession.TryGetComponent(out Throwable throwable))
        {
            currentThrowable = throwable;
        }
        _aimMode.ExitAimMode();
    }

    public void Unpossess()
    {
        if (CurrentPossession != null)
        {
            CurrentPossession.GetComponent<IPossessable>().Unpossess();
            CurrentPossession = null;
            currentThrowable = null;
            _aimMode.ExitAimMode();
            _thirdPersonController.freeze = false;
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
