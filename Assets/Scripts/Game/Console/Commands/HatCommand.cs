using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hat Command", menuName = "Utilities/ConsoleCommands/HatCommand")]
public class HatCommand : ConsoleCommand
{
    [SerializeField] GameObject hat;
    public override bool Process(string[] args)
    {
        
        GameObject headbone = GameObject.Find("headbone");
        if (headbone != null)
        {
            Vector3 offset = new Vector3(0, 0.15f, 0);
            GameObject coolHat = Instantiate(original: hat, parent: headbone.transform, position: headbone.transform.position + offset, rotation: Quaternion.Euler(-90, 0, 0));
        }
        return true;
    }
}
