using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]
public class NavMeshPathVisualizer : MonoBehaviour
{
    private NavMeshAgent agent;
    private LineRenderer lineRenderer;

    [Header("Configuración")]
    public Color pathColor = Color.green; // Color de la línea
    public float lineHeightOffset = 0.1f; // Altura para evitar z-fighting

    void Start()
    {
        // Obtener componentes
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();

        // Configurar LineRenderer
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")) {
            color = pathColor
        };
    }

    void Update()
    {
        // Actualizar el camino visual
        DrawPath();
    }

    void DrawPath()
    {
        if (agent.hasPath && agent.path.corners.Length > 0)
        {
            // Obtener los puntos del camino
            Vector3[] corners = agent.path.corners;
            
            // Configurar el LineRenderer
            lineRenderer.positionCount = corners.Length;
            
            // Añadir puntos al LineRenderer con offset de altura
            for (int i = 0; i < corners.Length; i++)
            {
                lineRenderer.SetPosition(i, corners[i] + Vector3.up * lineHeightOffset);
            }
        }
        else
        {
            // Si no hay camino, ocultar la línea
            lineRenderer.positionCount = 0;
        }
    }

    // Método para cambiar el color dinámicamente
    public void SetPathColor(Color newColor)
    {
        lineRenderer.material.color = newColor;
    }
}