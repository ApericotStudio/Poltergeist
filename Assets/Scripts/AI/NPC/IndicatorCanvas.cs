using UnityEngine;

public class IndicatorCanvas : MonoBehaviour
{
    private void Update()
    {
        RotateCanvasTowardsPlayer();
    }

    private void RotateCanvasTowardsPlayer()
    {
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-directionToCamera);
    }
}
