using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotationSpeed = 80f;
    public float heightOffset = 0.1f;
    public PolygonCollider2D terrainCollider;
    public Transform turretTransform;
    public LayerMask groundLayer;

    private Vector2 targetPosition;
    private Vector2 prevPosition;

    void Start()
    {
        prevPosition = transform.position;
    }

    void Update()
    {
        MoveTank();
        RotateTurret();
    }

    void MoveTank()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float rotateInput = Input.GetAxis("Horizontal");

        RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x+forwardInput*0.15f, transform.position.y + heightOffset, transform.position.z), Vector2.down, heightOffset*2, groundLayer);

        if (hit.collider == terrainCollider)
        {
            targetPosition = hit.point;
            transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
            transform.Translate(forwardInput * moveSpeed * Time.deltaTime, 0, 0);
            CalculateNormalAndAlign();
            prevPosition = transform.position;
        }
        else
        {
            transform.position = new Vector3(prevPosition.x, prevPosition.y, transform.position.z);
        }

    }

    void RotateTurret()
    {
        float rotateInput = Input.GetAxis("Horizontal");
        turretTransform.Rotate(Vector3.forward, rotateInput * rotationSpeed * Time.deltaTime);
    }

    void CalculateNormalAndAlign()
    {
        Vector2[] points = terrainCollider.points;
        int index = FindClosestPointIndex();

        Vector2 p1 = terrainCollider.transform.TransformPoint(points[index % points.Length]);
        Vector2 p2;
        Vector2 pr = terrainCollider.transform.TransformPoint(points[(index + 1) % points.Length]);
        Vector2 pl = terrainCollider.transform.TransformPoint(points[(index - 1) % points.Length]);

        if (Vector2.Distance(transform.position, pr) > Vector2.Distance(transform.position, pl)) p2 = pl;
        else p2 = pr;
        Vector3 normal = CalculatePlaneNormal(p1, p2);
        Debug.Log(normal);
        Quaternion q = Quaternion.FromToRotation(Vector3.up, normal);
        transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
    }

    int FindClosestPointIndex()
    {
        float minDistance = float.MaxValue;
        int index = 0;
        Vector2 pos = transform.position;

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
        Vector3 v2 = new Vector3(1, 0, 1);
        return Vector3.Cross(v1, v2).normalized;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPosition, 0.2f);
    }
}