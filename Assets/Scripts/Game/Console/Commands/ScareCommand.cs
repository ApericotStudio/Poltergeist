using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scare Command", menuName = "Utilities/ConsoleCommands/ScareCommand")]
public class ScareCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        if (args.Length != 1) { return false; }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) { return false; }
        switch (args[0])
        {
            case "small":
                break;
            case "medium":
                break;
            case "big":
                Interactable interactable = player.GetComponentInChildren<Interactable>();
                interactable.Use();
                ObservableObject observable = player.GetComponentInChildren<ObservableObject>();
                observable.GeistCharge = 1;
                break;
        }
        return true;
    }
}
