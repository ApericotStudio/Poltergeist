public interface IState
{
    void Handle();
    void StopStateCoroutines();
}
