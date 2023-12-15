/// <summary>
/// The Visitors's senses. Handles the Visitors's field of view, hearing radius, and detection radius.
/// </summary>
public class VisitorSenses : BaseSenses
{
    private FearHandler _fearHandler;

    protected override void Awake()
    {
        base.Awake();
        _fearHandler = GetComponent<FearHandler>();
    }

    public override void OnNotify(ObservableObject observableObject)
    {
        if (!DetectedObjects.TryGetValue(observableObject, out var detectedProperties))
            return;
        
        _fearHandler.Handle(observableObject, detectedProperties);
    }
}
