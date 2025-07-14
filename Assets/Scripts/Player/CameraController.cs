using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public float followSpeed = 10f;
    public float zoomOut = 10f;
    public float zoomIn = 5f;
    public float zoomTransitionDuration = 1f;
    public bool aimingModeActive = false;

    private float coof = 0.4f;
    private float currentZoomDistance;
    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        FollowTarget();
        ZoomCamera();
    }

    void FollowTarget()
    {
        Vector3 aiming = (player.position + enemy.position) / 2;
        Vector3 focusPoint = aimingModeActive ? aiming : player.position;
        focusPoint.z = -10;
        focusPoint += new Vector3(0, 2, 0);
        transform.position = Vector3.Lerp(transform.position, focusPoint, followSpeed * Time.deltaTime);
    }

    void ZoomCamera()
    {
        zoomOut = Mathf.Max(Vector2.Distance(player.position, enemy.position) * coof, zoomIn);
        float targetZoomDistance = aimingModeActive ? zoomOut : zoomIn;
        currentZoomDistance = Mathf.Lerp(currentZoomDistance, targetZoomDistance, Time.deltaTime / zoomTransitionDuration);
        camera.orthographicSize = currentZoomDistance;
    }

    public void ToggleAimingMode(bool enable)
    {
        aimingModeActive = enable;
    }
}

