using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsoleCommand
{
    public string CommandWord { get; }
    public bool Process(string[] args);
}
