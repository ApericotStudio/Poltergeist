using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The various states that the clutter can be in.
/// </summary>
public enum ClutterState
{
    Idle,
    Falling,
    Sliding,
    Destroyed
}
public enum ClutterType
{
    Big,
    Small
}
/// <summary>
/// The Clutter class is used to store the state of the clutter. 
/// It also contains the anxiety values that will be added to the NPC's anxiety when the clutter has entered certain states.
/// </summary>
public class Clutter : MonoBehaviour
{
    [Header("Clutter Settings")]
    [Tooltip("This value gets added to the NPC's anxiety when the clutter moves.")]
    public float MoveAnxietyValue = 2f;
    [Tooltip("This value gets added to the NPC's anxiety when the clutter is destroyed.")]
    public float DestroyAnxietyValue = 10f;
    [Tooltip("The value that gets applied to the NPC's anxiety when the clutter is audible.")]
    [Range(1f, 10f)]
    public float AuditoryAnxietyValue = 1f;
    [Tooltip("The value that gets applied to the NPC's anxiety when the clutter is visible.")]
    [Range(1f, 10f)]
    public float VisualAnxietyValue = 3f;
    [SerializeField]
    [Tooltip("The type of clutter.")]
    private ClutterType _clutterType;
    [SerializeField]
    [Tooltip("The value that gets added to the NPC's anxiety when the clutter is small.")]
    private float _smallClutterAnxietyValue = 1f;
    [SerializeField]
    [Tooltip("The value that gets added to the NPC's anxiety when the clutter is big.")]
    private float _bigClutterAnxietyValue = 3f;


    private ClutterState _state = ClutterState.Idle;

    private bool _isAudible = false;
    private bool _isVisible = false;

    public bool IsVisible { get => _isVisible; set => _isVisible = value; }
    public bool IsAudible { get => _isAudible; set => _isAudible = value; }

    public ClutterState State { get => _state; set => _state = value; }
    public ClutterType Type { get => _clutterType; set => _clutterType = value; }
    public float SmallClutterAnxietyValue { get => _smallClutterAnxietyValue; set => _smallClutterAnxietyValue = value; }
    public float BigClutterAnxietyValue { get => _bigClutterAnxietyValue; set => _bigClutterAnxietyValue = value; }
}
