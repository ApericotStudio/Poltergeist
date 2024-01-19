using UnityEngine;
using UnityEngine.UI;

public class VisitorOverlayController : MonoBehaviour
{
    private FearHandler _fearHandler;
    private InGameUIController _inGameUIController;

    [SerializeField] private Image _face;
    [SerializeField] private Image _filler;
    [SerializeField] private Image _phobiaBadge;

    [SerializeField] private Sprite _investigateFace;
    [SerializeField] private Sprite _panickedFace;
    [SerializeField] private Sprite _restingFace;
    [SerializeField] private Sprite _scaredFace;

    public void Setup(VisitorController visitorController, InGameUIController inGameUIController)
    {
        Setup(visitorController);
        _inGameUIController = inGameUIController;
    }

    public void Setup(VisitorController visitorController)
    {
        visitorController.OnStateChange.AddListener(OnVisitorStateChanged);
        visitorController.OnFearValueChange.AddListener(OnFearValueChanged);
        _fearHandler = visitorController.gameObject.GetComponent<FearHandler>();
        _fearHandler.OnObjectPhobia += OnPhobiaScare;
    }

    public void OnPhobiaScare(ObservableObject observableObject)
    {
        _fearHandler.OnObjectPhobia -= OnPhobiaScare;
        _phobiaBadge.gameObject.SetActive(true);
        if (_inGameUIController != null)
        {
            _inGameUIController.ShowNotification("Phobia discovered!", 6);
        }
    }

    private void OnFearValueChanged(float fearvalue, float feardifference, VisitorController controller)
    {
        _filler.fillAmount = fearvalue / 100;
    }

    private void OnVisitorStateChanged(IState state)
    {
        Sprite face = _restingFace;
        switch (state)
        {
            case (InvestigateState):
                face = _investigateFace;
                break;
            case (PanickedState):
                face = _panickedFace;
                break;
            case (ScaredState):
                face = _scaredFace;
                break;
        }
        _face.sprite = face;
    }
}
