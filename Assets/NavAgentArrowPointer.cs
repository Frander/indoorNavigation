using UnityEngine;
using UnityEngine.AI;

public class ArrowPointerFollowCamera : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform arrowTransform;
    public Camera mainCamera;
    public float distanceFromCamera = 2f;
    public float hideDistance = 1f; // Distancia mínima para ocultar la flecha

    void LateUpdate()
    {
        if (agent == null || arrowTransform == null || mainCamera == null)
            return;

        // Calcular distancia al destino
        float distanceToDestination = Vector3.Distance(agent.transform.position, agent.destination);

        // Ocultar flecha si ya está muy cerca
        if (distanceToDestination <= hideDistance)
        {
            arrowTransform.gameObject.SetActive(false);
            return;
        }
        else
        {
            arrowTransform.gameObject.SetActive(true);
        }

        // Posicionar flecha frente a la cámara
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 arrowPosition = cameraPosition + cameraForward * distanceFromCamera;
        arrowTransform.position = arrowPosition;

        // Dirección horizontal hacia el destino
        Vector3 toDestination = agent.destination - cameraPosition;
        toDestination.y = 0f;

        if (toDestination.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(toDestination.x, toDestination.z) * Mathf.Rad2Deg;
            arrowTransform.rotation = Quaternion.Euler(90f, 90f, -angle);
        }
    }
}
