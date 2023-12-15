using UnityEngine;
using UnityEngine.UI;

public class FearBarController : MonoBehaviour
{
    private Slider _fearBar;

    private readonly float _maxFear = 100;
    private NpcController _npcController;

    private void Awake()
    {
        _fearBar = GetComponent<Slider>();
        _npcController = GetComponentInParent<NpcController>();
    }

    private void Start()
    {
        _npcController.OnFearValueChange.AddListener(OnFearChange);
    }

    public void OnFearChange(float fear)
    {
        _fearBar.value = fear/_maxFear;
    }
}
