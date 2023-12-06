using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Drop : MonoBehaviour
{

    private HingeJoint _hingeJoint;
    [SerializeField]
    private GameObject _connectedBody;
    private void Awake()
    {
        _hingeJoint = GetComponent<HingeJoint>();
    }
    public void Activate()
    {
        _hingeJoint.connectedBody = _connectedBody.GetComponent<Rigidbody>();
        _connectedBody.SetActive(false);
    }
}
