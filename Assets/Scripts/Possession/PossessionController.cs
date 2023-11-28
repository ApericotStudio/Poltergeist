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
    private AudioSource _audioSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip _possessSound;
    [SerializeField] private AudioClip _unpossessSound;

    private void Awake()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _visionController = GetComponent<VisionController>();
        _audioSource = GetComponent<AudioSource>();
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
            RemovePossessionObjects();
        }
        if (objectInView.GetComponent<ObservableObject>().State == ObjectState.Broken)
        {
            return;
        }
        CurrentPossession = objectInView;
        possessable.Possess();
        CurrentPossession.GetComponent<ObservableObject>().AddObserver(this);
        _thirdPersonController.freeze = true;
        _audioSource.PlayOneShot(_possessSound);
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
            RemovePossessionObjects();
            _thirdPersonController.freeze = false;
            _audioSource.PlayOneShot(_unpossessSound);
            CurrentPossessionChanged?.Invoke();
        }
    }

    private void RemovePossessionObjects()
    {
        CurrentPossession.GetComponent<IPossessable>().Unpossess();
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
