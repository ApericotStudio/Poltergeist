using UnityEngine;
using UnityEngine.UI;

public class VisitorOverlayController : MonoBehaviour
{
    [SerializeField] private Image _face;

    [SerializeField] private Sprite _investigateFace;
    [SerializeField] private Sprite _panickedFace;
    [SerializeField] private Sprite _restingFace;
    [SerializeField] private Sprite _scaredFace;

    public void Setup(VisitorController visitorController)
    {
        visitorController.OnStateChange.AddListener(OnVisitorStateChanged);
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
