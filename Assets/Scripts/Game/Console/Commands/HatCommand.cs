using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hat Command", menuName = "Utilities/ConsoleCommands/HatCommand")]
public class HatCommand : ConsoleCommand
{
    [SerializeField] List<GameObject> hats;
    public override bool Process(string[] args)
    {
        
        GameObject headbone = GameObject.Find("headbone");
        if (headbone != null)
        {
            System.Random random = new System.Random();
            int index = random.Next(hats.Count);
            GameObject hat = hats[index];
            //Vector3 offset = new Vector3(0, 0.15f, 0);
            GameObject coolHat = Instantiate(original: hat, parent: headbone.transform, position: headbone.transform.position, rotation: Quaternion.identity);
            return true;
        }
        return false;
    }
}
