using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] // Asegura que el GameObject tenga NavMeshAgent
public class AgentController : MonoBehaviour
{
    [Header("Configuración")]
    public Transform target; // Opcional: Asignar un objetivo desde el Inspector
    private NavMeshAgent agent;

    void Start()
    {
        // Obtener el componente NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
                    agent.SetDestination(target.position);

    }

    // void Update()
    // {
    //     // Si hay un target asignado, mover hacia él
    //     if (target != null)
    //     {
    //         agent.SetDestination(target.position);
    //     }
    //     // Si no hay target, mover al clickear
    //     else
    //     {
    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             MoveToMousePosition();
    //         }
    //     }
    // }

    // Mueve el agente a la posición del clic del mouse
    void MoveToMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Verificar si el punto está en el NavMesh
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
        }
    }

    // Método para cambiar el destino por código
    public void SetNewDestination(Vector3 newPosition)
    {
        agent.SetDestination(newPosition);
    }
}