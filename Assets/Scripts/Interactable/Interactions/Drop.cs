using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Drop : MonoBehaviour
{

    private SpringJoint _springJoint;
    private void Awake()
    {
        _springJoint = GetComponent<SpringJoint>();
    }
    public void Activate()
    {
        _springJoint.connectedBody.gameObject.SetActive(false);

    }
}
