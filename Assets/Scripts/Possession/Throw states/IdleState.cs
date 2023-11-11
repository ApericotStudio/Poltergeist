public class IdleState : IThrowState
{
    private Throwable controller;

    public IdleState(Throwable controller)
    {
        this.controller = controller;
    }

    public void OnStateEnter()
    {
        // do nothing
    }

    public void OnStateLeave()
    {
        // do nothing
    }

    public void OnAim()
    {
        controller.SetThrowState(controller.AimState);
    }

    public void OnStopAim()
    {
        // do nothing
    }

    public void Throw()
    {
        // do nothing
    }
}
