using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Realtor Disable Command", menuName = "Utilities/ConsoleCommands/RealtorDisableCommand")]
public class RealtorDisable : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        GameObject realtor = GameObject.Find("RealtorArmature");
        if (realtor == null) { return false; }
        RealtorSenses senses = realtor.GetComponent<RealtorSenses>();
        if (args.Length == 0){ senses.Disabled = true; }
        if (args[0] == "false") { senses.Disabled = false; }
        else { senses.Disabled = true; }
        return true;
    }
}
