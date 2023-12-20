using UnityEngine;
using UnityEngine.UI;

public class FearBarController : MonoBehaviour
{
    private Slider _fearBar;

    private readonly float _maxFear = 100;
    private VisitorController _visitorController;

    private void Awake()
    {
        _fearBar = GetComponent<Slider>();
        _visitorController = GetComponentInParent<VisitorController>();
    }

    private void Start()
    {
        _visitorController.OnFearValueChange.AddListener(OnFearChange);
    }

    public void OnFearChange(float fear)
    {
        _fearBar.value = fear/_maxFear;
    }
}
