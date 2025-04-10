using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PlacePrefabAtDistance : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    public GameObject arrow;
    public ArrowPointerFollowCamera arrowData;

   // public GameObject canvasSub;
    // public GameObject instructions;
    public Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f); // Offset in X and Z axes
    GameObject instantiatedPrefab;
    public ARTrackedImageManager _arTrackedImageManager;
    private bool isActive = false;
    public string name;

    private void Awake()
    {
        //_arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        //_arTrackedImageManager.enabled = true;
        //SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        Debug.Log("Unsubscribed from events");
        _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
        //_arTrackedImageManager.enabled = false;
    }

    public void exitSubs(){
        _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;

    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (var trackedImage in obj.added)
        {
            InstantiatePrefab(trackedImage);
        }
        foreach (var trackedImage in obj.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                InstantiatePrefab(trackedImage);
            }
        }
        foreach (var trackedImage in obj.removed)
        {
            Debug.Log("REMOVED");
            Destroy(instantiatedPrefab);
            isActive = false;
        }
    }

    void InstantiatePrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        Debug.Log(imageName);
        if(!isActive && name == imageName)
        {
            StartCoroutine(ShowAnimation3D(trackedImage));
        }
        else{
            UpdatePrefabPosition(trackedImage);
        } 
    }

    void UpdatePrefabPosition(ARTrackedImage trackedImage)
    {
        // if (instantiatedPrefab != null)
        // {
        //      instantiatedPrefab.transform.position = trackedImage.transform.position;
        // }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // Desuscribirse de los eventos antes de cambiar de escena
        _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    IEnumerator ShowAnimation3D(ARTrackedImage trackedImage)
    {
        //instructions.SetActive(true);
        isActive = true;
        yield return new WaitForSeconds(1.5f);
        instantiatedPrefab = Instantiate(prefabToInstantiate);
        instantiatedPrefab.transform.position = trackedImage.transform.position + offset;
        if (instantiatedPrefab.GetComponent<ARAnchor>() == null)
        {
            instantiatedPrefab.AddComponent<ARAnchor>();
        }
        arrowData.agent = instantiatedPrefab.transform.Find("agent").GetComponent<UnityEngine.AI.NavMeshAgent>();
                arrow.SetActive(true);

       // canvasSub.SetActive(true);
        
        
    }
}
