using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Reset Command", menuName = "Utilities/ConsoleCommands/ResetCommand")]
public class ResetCommand : ConsoleCommand
{
    [SerializeField] private string[] _saveFileNames;
    
    public override bool Process(string[] args)
    {
        foreach(string fileName in _saveFileNames)
        {
            string fullFileName = Application.persistentDataPath + "/" + fileName;
            if (File.Exists(fullFileName)) 
            {
                File.Delete(fullFileName);
            }
        }
        return true;
    }
}
