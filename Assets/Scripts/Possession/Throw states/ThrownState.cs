using UnityEngine;

public class ThrownState : IThrowState, IObserver
{
    private Throwable controller;
    private ObservableObject observableObject;
    private bool isAiming = false;

    public ThrownState(Throwable controller)
    {
        this.controller = controller;
    }

    public void OnStateEnter()
    {
        observableObject = controller.GetComponent<ObservableObject>();
        observableObject.AddObserver(this);
    }

    public void OnStateLeave()
    {
        Debug.Log("before");
        observableObject.RemoveObserver(this);
        Debug.Log("after");
    }

    public void OnAim()
    {
        isAiming = true;
    }

    public void OnStopAim()
    {
        isAiming = false;
    }

    public void Throw()
    {
        // do nothing
    }

    public void OnNotify(ObservableObject observableObject)
    {
        if (observableObject.State == ObjectState.Idle)
        {
            if (isAiming)
            {
                controller.SetThrowState(controller.AimState);
            }
            else
            {
                controller.SetThrowState(controller.IdleState);
            }
        }
    }
}
