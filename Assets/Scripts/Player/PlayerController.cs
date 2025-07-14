using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveRotationSpeed = 15f;
    public float rotationSpeed = 80f;
    public float heightOffset = 0.1f;
    public float facility = 0.1f;
    public PolygonCollider2D terrainCollider;
    public Transform turretTransform;
    public LayerMask groundLayer;
    public CameraController cameraController;

    private Vector2 targetPosition;
    private Vector2 prevPosition;

    void Start()
    {
        transform.position = new Vector3(terrainCollider.points[25].x, terrainCollider.points[25].y, 0);
        prevPosition = transform.position;
    }

    void Update()
    {
        MoveTank();
        RotateTurret();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            cameraController.aimingModeActive = !cameraController.aimingModeActive;
        }
    }


    void MoveTank()
    {
        float forwardInput = Input.GetAxis("Horizontal");

        RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + forwardInput * facility, transform.position.y + heightOffset, transform.position.z), Vector2.down, heightOffset*2, groundLayer);

        if (hit.collider == terrainCollider)
        {
            targetPosition = hit.point;
            transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
            transform.Translate(forwardInput * moveSpeed * Time.deltaTime, 0, 0);
            CalculateNormalAndAlign(targetPosition);
            prevPosition = transform.position;
        }
        else
        {
            transform.position = new Vector3(prevPosition.x, prevPosition.y, transform.position.z);
            CalculateNormalAndAlign(targetPosition);
        }

    }

    void RotateTurret()
    {
        float rotateInput = Input.GetAxis("Vertical");
        turretTransform.Rotate(Vector3.forward, rotateInput * rotationSpeed * Time.deltaTime);
    }

    void CalculateNormalAndAlign(Vector2 p0)
    {
        Vector2[] points = terrainCollider.points;
        int index = FindClosestPointIndex(p0);

        Vector2 p1 = terrainCollider.transform.TransformPoint(points[index % points.Length]);
        Vector3 normal = CalculatePlaneNormal(p0, p1);
        Quaternion q = Quaternion.FromToRotation(Vector3.up, normal);
        transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * moveRotationSpeed * GetCurrentSpeed());
    }

    int FindClosestPointIndex(Vector2 pos)
    {
        float minDistance = float.MaxValue;
        int index = 0;
        for (int i = 0; i < terrainCollider.points.Length; i++)
        {
            float d = Vector2.Distance(pos, terrainCollider.transform.TransformPoint(terrainCollider.points[i]));
            if (d < minDistance)
            {
                minDistance = d;
                index = i;
            }
        }
        return index;
    }

    Vector3 CalculatePlaneNormal(Vector2 p1, Vector2 p2)
    {
        Vector3 v1 = new Vector3(p2.x - p1.x, p2.y - p1.y, 0);
        Vector3 v2 = new Vector3(0, 0, 1);
        Vector3 normal = Vector3.Cross(v1, v2).normalized;
        if (normal.y < 0) normal *= -1f;
        return normal;
    }
    public float GetCurrentSpeed()
    {
        Vector2 currentPosition = transform.position;
        float distanceTraveled = Vector2.Distance(currentPosition, prevPosition);
        prevPosition = currentPosition;
        return distanceTraveled / Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPosition, 0.2f);
    }
}