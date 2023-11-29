using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Drop : MonoBehaviour
{

    private SpringJoint _springJoint;
    private Rigidbody _rigidBody;
    private void Awake()
    {
        _springJoint = GetComponent<SpringJoint>();
        _rigidBody = GetComponent<Rigidbody>();
    }
    public void Activate()
    {
        _springJoint.spring = 0;
        _rigidBody.AddForce(new Vector3(0, -1, 0));
        _springJoint.connectedBody.gameObject.SetActive(false);
    }
}
