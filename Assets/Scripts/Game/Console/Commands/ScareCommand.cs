using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scare Command", menuName = "Utilities/ConsoleCommands/ScareCommand")]
public class ScareCommand : ConsoleCommand
{
    [SerializeField] private FloatReference _sizeFearSmall;
    [SerializeField] private FloatReference _sizeFearMedium;
    [SerializeField] private FloatReference _sizeFearBig;

    public override bool Process(string[] args)
    {
        GameObject cheatScareObject = GameObject.Find("CheatScareObject");
        if (args.Length != 1 || cheatScareObject == null) { return false; }
        Interactable interactable = cheatScareObject.GetComponent<Interactable>();
        ObservableObject observable = cheatScareObject.GetComponent<ObservableObject>();
        switch (args[0])
        {
            case "small":
                observable.SizeFear = _sizeFearSmall;
                break;
            case "medium":
                observable.SizeFear = _sizeFearMedium;
                break;
            case "big":
                observable.SizeFear = _sizeFearBig;
                break;
            default:
                observable.SizeFear = _sizeFearMedium;
                break;
        }
        interactable.Use();
        observable.GeistCharge = 1;
        return true;
    }
}
