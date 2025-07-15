using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeCutter : MonoBehaviour
{
    private PolygonCollider2D landscape;
    private LandscapeGenerator generator;
    public int segments = 45;
    public float radius = 2f;

    public void Cut()
    {
        landscape = FindObjectOfType<PolygonCollider2D>();
        generator = FindObjectOfType<LandscapeGenerator>();
        Vector2 center = transform.position;
        Vector2[] points = landscape.points;
        Vector2 firstPoint = Vector2.zero, lastPoint = Vector2.zero;
        int firstPointIndex = -1, lastPointIndex = -1;
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector2 result = FindIntersection(points[i], points[i + 1], center, radius);
            if (result != Vector2.negativeInfinity)
            {
                Debug.Log(result);
                if (firstPointIndex == -1)
                {
                    firstPointIndex = i;
                    firstPoint = result;
                }
                else
                {
                    lastPointIndex = i;
                    lastPoint = result;
                    break;
                }
            }
        }
        Vector2[] arc = GenerateArc(firstPoint, lastPoint, center);
        Debug.Log($"ArcLength = {arc.Length}, pointsLebgth = {points.Length}, ceneter = {center}");
        Vector2[] newPoints = new Vector2[points.Length - (lastPointIndex - firstPointIndex - 1) + arc.Length];
        Debug.Log($"newpointsLength = {newPoints.Length}");
        Debug.Log($"from = {firstPointIndex}, to = {lastPointIndex}");
        int offset = 0;
        for (int i = 0; i < points.Length; i++)
        {
            if (i >= firstPointIndex && i < lastPointIndex)
            {
                for (int j = 0; j < arc.Length; j++)
                {
                    newPoints[i + j] = arc[j];
                    Debug.Log($"!!! newPoints[{i + j}] = {arc[j]}");
                }
                i = lastPointIndex;
                offset = arc.Length;
            }
            newPoints[i + offset] = points[i];
            Debug.Log($"newPoints[{i + offset}] = {points[i]}");
        }
        generator.UpdateLandscape(newPoints);
    }

    Vector2[] GenerateArc(Vector2 startPoint, Vector2 endPoint, Vector2 center)
    {
        float startAngle = Mathf.Atan2(startPoint.y - center.y, startPoint.x - center.x);
        float endAngle = Mathf.Atan2(endPoint.y - center.y, endPoint.x - center.x);

        while (startAngle > endAngle) endAngle += 2 * Mathf.PI;

        Vector2[] arcPoints = new Vector2[segments + 1];

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float angle = Mathf.Lerp(startAngle, endAngle, t);

            float x = center.x + radius * Mathf.Cos(angle);
            float y = center.y + radius * Mathf.Sin(angle);

            arcPoints[i] = new Vector2(x, y);
        }
        return arcPoints;
    }

    public Vector2 FindIntersection(Vector2 P1, Vector2 P2, Vector2 Center, float Radius)
    {
        float dx = P2.x - P1.x;
        float dy = P2.y - P1.y;
        float cx = Center.x;
        float cy = Center.y;
        float xc = P1.x;
        float yc = P1.y;
        float r = Radius;
        float a = dx * dx + dy * dy;
        float b = 2 * (dx * (xc - cx) + dy * (yc - cy));
        float c = (xc - cx) * (xc - cx) + (yc - cy) * (yc - cy) - r * r;

        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            return Vector2.negativeInfinity;
        }
        float sqrtDiscr = Mathf.Sqrt(discriminant);
        float t1 = (-b - sqrtDiscr) / (2 * a);
        float t2 = (-b + sqrtDiscr) / (2 * a);
        float t = Mathf.Clamp01(Mathf.Min(t1, t2));
        return new Vector2(P1.x + t * dx, P1.y + t * dy);
    }
}
