using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Swing : MonoBehaviour
{
    [SerializeField]
    private Vector3 _swingDirection;

    public void Activate()
    {
        // Swings object in direction of player
        gameObject.GetComponent<Rigidbody>().AddForce(_swingDirection, ForceMode.Impulse);
    }
}
