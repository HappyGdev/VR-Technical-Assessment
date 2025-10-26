using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playerController : MonoBehaviour
{
    #region Variables
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [Space]

    private Rigidbody rb;
    private Camera mainCam;
    private Vector3 inputDir;

    #endregion

    #region Initilize
    private void Awake()
    {
        //Component Shouldn't Call in Update or fixedUpdate, i called them on Awake
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
    }
    #endregion

    #region update
    private void Update()
    {
        // Get input in Update not Fixed Update cause Maybe Lose Frame
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        //inoredr to call inputdir we make it as variable
        inputDir = new Vector3(inputX, 0f, inputZ).normalized;
    }
    void FixedUpdate()
    {
        //Physic Based Move in Fixed Update
        HandleMove();
        HandleRotation();
    }
    #endregion

    #region Move
    private void HandleMove()
    {
        if (inputDir.magnitude >= 0.01f)
        {
            Vector3 move = inputDir * moveSpeed;
            rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
        }
    }
    #endregion

    #region Rotation
    private void HandleRotation()
    {
        Vector3 worldPos = GetMouseWorldPositionAtPlayerHeight();
        Vector3 direction = worldPos - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }
    #endregion

    #region Mouse
    Vector3 GetMouseWorldPositionAtPlayerHeight()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return transform.position + transform.forward;
    }
    #endregion
}