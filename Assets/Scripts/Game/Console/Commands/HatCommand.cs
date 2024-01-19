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
            GameObject coolHat = Instantiate(original: hat, parent: headbone.transform, position: headbone.transform.position, rotation: Quaternion.identity);
            coolHat.transform.forward = headbone.transform.forward;
            CheckForHatAchievement();
            return true;
        }
        return false;
    }

    private void CheckForHatAchievement()
    {
        if (!SteamManager.Initialized) return;

        Steamworks.SteamUserStats.GetAchievement("Mad Hatter", out bool achieved);
        if (!achieved)
        {
            Steamworks.SteamUserStats.SetAchievement("Mad Hatter");
            Steamworks.SteamUserStats.StoreStats();
        }
    }
}
