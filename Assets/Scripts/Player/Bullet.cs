using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speedX;
    public float speedY;
    public float startAngle;
    public float gravity;
    public float radius;
    private Vector2 velocity;
    public Transform player;
    public ParticleSystem explosion;

    void Update()
    {
        velocity = new Vector2(speedX * Time.deltaTime, speedY * Time.deltaTime - gravity * Time.deltaTime * Time.deltaTime);
        speedY -= gravity * Time.deltaTime;
        transform.Translate(velocity, Space.World);
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, velocity.normalized, 0.2f);

        if (hit.collider != null)
        {
            if (hit.collider.tag != "Player")
            {
                LandscapeCutter comp = gameObject.AddComponent<LandscapeCutter>();
                gameObject.transform.SetParent(FindObjectOfType<LandscapeGenerator>().gameObject.transform);
                comp.radius = radius;
                comp.Cut();
                gameObject.SetActive(false);
                ParticleSystem exp = GameObject.Instantiate(explosion);
                exp.transform.SetParent(transform.parent);
                exp.transform.position = transform.position;
                Destroy(exp.gameObject, 2f);
                Destroy(gameObject, 2f);
            }
        }
    }

    private void OnDestroy()
    {
        Camera.main.GetComponent<CameraController>().player = player;
    }
}
