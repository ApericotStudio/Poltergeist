public class ThrownState : IThrowState, IObserver
{
    private Throwable controller;
    private ObservableObject observableObject;

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
        observableObject.RemoveObserver(this);
    }

    public void OnAim()
    {
        // do nothing
    }

    public void OnStopAim()
    {
        // do nothing
    }

    public void Throw()
    {
        // do nothing
    }

    public void OnNotify(ObservableObject observableObject)
    {
        if (observableObject.State == ObjectState.Idle)
        {
            controller.SetThrowState(controller.IdleState);
        }
    }
}
