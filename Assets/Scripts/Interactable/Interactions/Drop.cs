using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Drop : MonoBehaviour
{

    private HingeJoint _hingeJoint;
    private Rigidbody _rigidBody;
    private ObservablePhysics observablePhysics;

    private void Awake()
    {
        observablePhysics = GetComponent<ObservablePhysics>();
        observablePhysics.FirstHit = false;
        _hingeJoint = GetComponent<HingeJoint>();
        _rigidBody = GetComponent<Rigidbody>();
    }
    public void Activate()
    {
        //_rigidBody.AddForce(new Vector3(0, -1, 0));
        _hingeJoint.connectedBody.gameObject.SetActive(false);
    }
}
