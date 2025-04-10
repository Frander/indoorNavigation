// using UnityEngine;
// using UnityEngine.AI;

// [RequireComponent(typeof(NavMeshAgent))]
// public class NavMeshPathVisualizer : MonoBehaviour
// {
//     private NavMeshAgent agent;

//     void Awake()
//     {
//         agent = GetComponent<NavMeshAgent>();
//     }

//     void OnDrawGizmos()
//     {
//         if (agent == null || agent.path == null)
//             return;

//         NavMeshPath path = agent.path;

//         // Color del path
//         Gizmos.color = Color.green;

//         // Dibujar las esquinas del path
//         for (int i = 0; i < path.corners.Length - 1; i++)
//         {
//             Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
//             Gizmos.DrawSphere(path.corners[i], 0.1f); // opcional: dibujar puntos
//         }

//         // Último punto
//         if (path.corners.Length > 0)
//             Gizmos.DrawSphere(path.corners[path.corners.Length - 1], 0.1f);
//     }
// }

using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]
public class NavMeshPathVisualizer : MonoBehaviour
{
    private NavMeshAgent agent;
    private LineRenderer lineRenderer;

    [Header("Configuración")]
    public Color pathColor = Color.green;
    public float lineHeightOffset = 0.1f;
    public float pointSpacing = 1.0f;    // Espacio entre puntos
    public float pointRadius = 0.1f;     // Radio del círculo

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")) {
            color = pathColor
        };
    }

    void Update()
    {
        DrawPath();
    }

    void DrawPath()
    {
        if (agent.hasPath && agent.path.corners.Length > 0)
        {
            Vector3[] pathCorners = agent.path.corners;
            List<Vector3> sampledPoints = SamplePointsAlongPath(pathCorners, pointSpacing);
            
            List<Vector3> linePoints = new List<Vector3>();
            
            // Generar círculos en cada punto muestreado
            foreach (var point in sampledPoints)
            {
                CreateCirclePoints(point + Vector3.up * lineHeightOffset, pointRadius, 12, ref linePoints);
            }

            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    // Muestrea puntos a lo largo del camino
    List<Vector3> SamplePointsAlongPath(Vector3[] corners, float spacing)
    {
        List<Vector3> sampledPoints = new List<Vector3>();
        float accumulatedDistance = 0f;

        for (int i = 0; i < corners.Length - 1; i++)
        {
            Vector3 segmentStart = corners[i];
            Vector3 segmentEnd = corners[i + 1];
            float segmentLength = Vector3.Distance(segmentStart, segmentEnd);

            while (accumulatedDistance + spacing <= segmentLength)
            {
                float t = accumulatedDistance / segmentLength;
                sampledPoints.Add(Vector3.Lerp(segmentStart, segmentEnd, t));
                accumulatedDistance += spacing;
            }
            accumulatedDistance -= segmentLength;
        }
        return sampledPoints;
    }

    // Genera los vértices de un círculo en XZ
    void CreateCirclePoints(Vector3 center, float radius, int segments, ref List<Vector3> points)
    {
        float angleStep = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 pos = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );
            points.Add(pos);
        }
        // Cerrar el círculo
        points.Add(points[points.Count - segments]);
    }
}
