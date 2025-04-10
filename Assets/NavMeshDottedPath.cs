using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshPathDotsRuntime : MonoBehaviour
{
    public GameObject spherePrefab;     // Prefab de la esfera
    public float spacing = 0.2f;        // Distancia entre puntos
    public float heightOffset = 0.1f;   // Altura en Y sobre el suelo

    private NavMeshAgent agent;
    private List<GameObject> dots = new List<GameObject>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Limpiar esferas anteriores
        foreach (var dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();

        if (agent.path == null || agent.path.corners.Length < 2)
            return;

        Vector3[] corners = agent.path.corners;

        for (int i = 0; i < corners.Length - 1; i++)
        {
            DrawDotsBetween(corners[i], corners[i + 1]);
        }
    }

    void DrawDotsBetween(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        Vector3 direction = (end - start).normalized;

        float drawn = 0f;
        while (drawn < distance)
        {
            Vector3 point = start + direction * drawn;
            point.y += heightOffset;

            GameObject dot = Instantiate(spherePrefab, point, Quaternion.identity);
            dot.transform.localScale = Vector3.one * 0.1f; // Tamaño pequeño opcional
            dots.Add(dot);

            drawn += spacing;
        }
    }
}
