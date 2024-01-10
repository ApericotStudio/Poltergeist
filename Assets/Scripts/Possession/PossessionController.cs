using UnityEngine;
using StarterAssets;
using UnityEngine.Events;
using TMPro;

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

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _hoverMessage;

    public delegate void Possession(int index);
    public event Possession hasPossessed;
    private int _amountofPossessions = 0;

    private bool _hasHit = false;
    private void Awake()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _visionController = GetComponent<VisionController>();
        _audioSource = GetComponent<AudioSource>();

        _visionController.LookingAtChanged.AddListener(HandleDisplayingPossessionPrompt);
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

        if (_amountofPossessions == 0)
        {
            hasPossessed?.Invoke(4);
            ++_amountofPossessions;
        }
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
            if (_hasHit)
            {
                hasPossessed?.Invoke(4 + _amountofPossessions);
            }
            _thirdPersonController.ToUnpossessLocation();
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
        if(observableObject.State == ObjectState.Hit)
        {
            if (_amountofPossessions == 1)
            {
                ++_amountofPossessions;
                hasPossessed?.Invoke(4 + _amountofPossessions);
                _hasHit = true;
            }
        }

        if (observableObject.State == ObjectState.Broken)
        {
            Unpossess();
        }
    }

    private void HandleDisplayingPossessionPrompt()
    {
        _hoverMessage.enabled = false;
        GameObject objectInView = _visionController.LookingAt;
        if (objectInView != null)
        {
            bool possessableAndNotBroken = objectInView.TryGetComponent(out Throwable throwable) && !throwable.isPossessed() && objectInView.TryGetComponent(out ObservableObject observableObject) && observableObject.State != ObjectState.Broken;
            if (possessableAndNotBroken)
            {
                _hoverMessage.enabled = true;
                _hoverMessage.text = "Press [E] to possess";
                return;
            }
        }
    }
}
