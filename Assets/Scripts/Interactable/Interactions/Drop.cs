using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Drop : MonoBehaviour
{
    [SerializeField]
    private Vector3 _swingDirection;

    private SpringJoint _springJoint;
    private void Awake()
    {
        _springJoint = GetComponent<SpringJoint>();
    }
    public void Activate()
    {
        // Swings object in direction of player
        //gameObject.GetComponent<Rigidbody>().AddForce(_swingDirection, ForceMode.Impulse);
        _springJoint.connectedBody.gameObject.SetActive(false);

    }
}
