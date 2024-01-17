using UnityEngine;

public class DynamicCanvas : MonoBehaviour
{
    [Header("Dynamic Canvas Settings")]
    [Tooltip("The transform that the canvas will follow."), SerializeField]
    private Transform _transformToFollow;
    
    private void Update()
    {
        RotateTowardsPlayer();
        if(_transformToFollow != null)
            FollowTransform();
    }

    private void RotateTowardsPlayer()
    {
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-directionToCamera);
    }

    private void FollowTransform()
    {
        transform.position = _transformToFollow.position;
    }
}
