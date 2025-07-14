using UnityEngine;

public class LandscapeGenerator : MonoBehaviour
{
    public float freq = 0.2f;
    public float diference = 1f;
    public float koof = 1f;
    public int stas = 3;
    public Vector2 size;

    void DotGenerator()
    {
        PolygonCollider2D colComponent = GetComponent<PolygonCollider2D>();
        float priv = 0;
        int vel = 0;
        Vector2[] dotCoords = new Vector2[(int)(size.x / freq) + 2];
        Vector3[] meshCoords = new Vector3[dotCoords.Length];
        dotCoords[0] = new Vector2(size.x / -2, -size.y);
        dotCoords[1] = new Vector2(size.x / -2, 0);
        dotCoords[dotCoords.Length - 1] = new Vector2(size.x / 2, -size.y);

        meshCoords[0] = new Vector3(size.x / -2, -size.y, 0);
        meshCoords[1] = new Vector3(size.x / -2, 0, 0);
        meshCoords[dotCoords.Length - 1] = new Vector3(size.x / -2, -size.y, 0);
        for (int x = 2; x < dotCoords.Length - 2; x++)
        {
            float randFloat;
            if (vel == 0) {
                randFloat = Random.Range(-diference, diference);
                vel = Random.Range(0, stas);
            } else {
                randFloat = Random.Range(0, priv * koof);
                vel--;
            }
            priv = randFloat;
            dotCoords[x] = new Vector2(dotCoords[x-1].x + freq, dotCoords[x-1].y + randFloat);
            meshCoords[x] = new Vector3(dotCoords[x - 1].x + freq, dotCoords[x - 1].y + randFloat, 0);
        }
        dotCoords[dotCoords.Length - 2] = new Vector2(size.x / 2, dotCoords[dotCoords.Length - 3].y);
        meshCoords[dotCoords.Length - 2] = new Vector3(size.x / -2, dotCoords[dotCoords.Length - 3].y, 0);
        colComponent.SetPath(0, dotCoords);
        Mesh mesh = colComponent.CreateMesh(true, false);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Awake()
    {
        DotGenerator();
    }
}
