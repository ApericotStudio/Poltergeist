using System.Collections.Generic;
using UnityEngine;

public class ScareCamController : MonoBehaviour
{
    [SerializeField] private Transform _scareCam;
    [SerializeField] private GameObject _scareCamImage;
    [SerializeField] private Transform _visitorCollection;
    private List<NpcController> _visitorsToShow = new();
    private Transform _visitorHeadToShow = null;

    private void Awake()
    {
        foreach (NpcController visitor in _visitorsToShow)
        {
            visitor.OnStateChange.AddListener(OnVisitorStateChange);
        }
    }

    private void OnVisitorStateChange(IState state)
    {
        foreach (NpcController visitor in _visitorCollection.GetComponentsInChildren<NpcController>())
        {
            if (_visitorsToShow.Contains(visitor))
            {
                if (!StateIsStateForScareCam(visitor.CurrentState))
                {
                    _visitorsToShow.Remove(visitor);
                }
            }
            else
            {
                if (StateIsStateForScareCam(visitor.CurrentState))
                {
                    _visitorsToShow.Add(visitor);
                }
            }
        }
    }

    private bool StateIsStateForScareCam(IState state)
    {
        return state is PanickedState || state is ScaredState;
    }

    private void Update()
    {
        if (_visitorHeadToShow == null)
        {
            _scareCamImage.SetActive(false);
            return;
        }
        _scareCamImage.SetActive(true);
        _scareCam.position = _visitorHeadToShow.position + new Vector3(0, 0, 1);
    }
}
