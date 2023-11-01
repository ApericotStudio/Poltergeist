using UnityEngine;

public class PossessionController : MonoBehaviour
{
    [Tooltip("Max range for possession")]
    [SerializeField, Range(0f, 20f)] private float possessionRange;

    private IPossessable currentPossession;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        currentPossession = GetComponent<IPossessable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Possess();
        }
    }

    /// <summary>
    /// If camera is looking at possessable object possess it and unpossess current possession
    /// </summary>
    private void Possess()
    {
        IPossessable possessable = LookForPossessableObject();
        if (possessable == null)
        {
            Unpossess();
            return;
        }
        currentPossession.Unpossess();
        possessable.Possess();
        currentPossession = possessable;
    }

    private void Unpossess()
    {
        currentPossession.Unpossess();
    }

    /// <summary>
    /// Checks if the mainCamera is pointing at an object with the IPossessable interface
    /// </summary>
    /// <returns>The found IPossessable interface or null when no IPossessable interface is found</returns>
    private IPossessable LookForPossessableObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, possessionRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out IPossessable possessableObject))
            {
                return possessableObject;
            }
        }
        return null;
    }
}
