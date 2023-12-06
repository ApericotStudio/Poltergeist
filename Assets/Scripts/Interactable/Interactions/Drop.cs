using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Drop : MonoBehaviour
{

    private HingeJoint _hingeJoint;
    public GameObject connectedBody;
    private void Awake()
    {
        _hingeJoint = GetComponent<HingeJoint>();
    }
    public void Activate()
    {
        //_rigidBody.AddForce(new Vector3(0, -1, 0));
        //_hingeJoint.connectedBody.gameObject.SetActive(false);
        _hingeJoint.connectedBody = connectedBody.GetComponent<Rigidbody>();
        connectedBody.SetActive(false);
    }
}
