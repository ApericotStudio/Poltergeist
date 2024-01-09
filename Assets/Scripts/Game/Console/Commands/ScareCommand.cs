using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scare Command", menuName = "Utilities/ConsoleCommands/ScareCommand")]
public class ScareCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        GameObject cheatScareObject = GameObject.Find("CheatScareObject");
        if (args.Length != 1 || cheatScareObject == null) { return false; }
        Interactable interactable = cheatScareObject.GetComponent<Interactable>();
        ObservableObject observable = cheatScareObject.GetComponent<ObservableObject>();
        switch (args[0])
        {
            case "small":
                observable.Type = ObjectType.Small;
                break;
            case "medium":
                observable.Type = ObjectType.Medium;
                break;
            case "big":
                observable.Type = ObjectType.Big;
                break;
            default:
                return false;
        }
        interactable.Use();
        observable.GeistCharge = 1;
        return true;
    }
}
