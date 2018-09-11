using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundSourceTranslationController : MonoBehaviour
{
    public LaserPointer laserPointer;
    public VoxelDrawing.VoxelDrawer voxelDrawer;
    public float soundSourceSpeed;
    public Transform center;
    public Transform leftFront, rightFront, leftBack, rightBack;//, left, right, front, back;
    public GameObject soundSourcePrefab;
    public GameObject buildingColliderPrefab;
    public GameObject campus;
    public LayerMask randomBuildingTranslationLayer;
    public LayerMask movingBuildingLayer;
    public AnimationCurve buildingTranslationTimeRandomVariableCurve;

    public GameObject SoundSource
    {
        get
        {
            return soundSource;
        }
    }

    [SerializeField]
    private GameObject soundSource;
    [SerializeField]
    private Coroutine currentTranslationCoroutine = null;
    // [SerializeField]
    // private Vector2 rectangleSize;
    // [SerializeField]
    // private Vector2[] rectangleCorners = new Vector2[4];
    [SerializeField]
    private List<GameObject> buildingsInCampus;
    private int eventCounter = 0;

    public static GameObject[] BuildingsInCampus {
        get {
            GameObject[] buildingsInCampusArray = new GameObject[instance.buildingsInCampus.Count];
            instance.buildingsInCampus.CopyTo(buildingsInCampusArray, 0);
            
            return buildingsInCampusArray;
        }
    }

    private Vector3[] translationDirectionsPool;

    private static SoundSourceTranslationController instance;

    void Awake() {
        instance = this;
    }

    void Start()
    {        
        // Find all buildings in campus model
        int modelsInCampus = campus.transform.childCount;
        for (int i = 0; i < modelsInCampus; i++)
        {
            Transform model = campus.transform.GetChild(i);

            if (model.name.Contains("Text") || model.name.Contains("Bridge") || model.name.Contains("Subregion") || model.name.Contains("Surface"))
            {
                continue;
            }

            if (model.gameObject.activeSelf)
            {
                buildingsInCampus.Add(model.gameObject);
            }
        }

        GenerateTranlsationDirectionsPool();
    }

    // Generates 100 evenly spaced direction vectors, 25 in each individual (N, E, W, S) direction
    private void GenerateTranlsationDirectionsPool()
    {
        translationDirectionsPool = new Vector3[100];

        Vector3 start = new Vector3(1f, 0f, 0f);
        Vector3 end = new Vector3(0f, 0f, 1f);
        float t = 1f / 26f;
        for (int i = 0; i < 25; i++)
        {
            translationDirectionsPool[i] = Vector3.Lerp(start, end, t * i).normalized;
        }

        Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
        for (int i = 0; i < 25; i++)
        {
            translationDirectionsPool[i + 25] = rotation * translationDirectionsPool[i];
        }

        rotation = Quaternion.Euler(0f, 180f, 0f);
        for (int i = 0; i < 25; i++)
        {
            translationDirectionsPool[i + 50] = rotation * translationDirectionsPool[i];
        }

        rotation = Quaternion.Euler(0f, 270f, 0f);
        for (int i = 0; i < 25; i++)
        {
            translationDirectionsPool[i + 75] = rotation * translationDirectionsPool[i];
        }

        // Debug.Log("Tranlsation Directions' Pool: " + Utils.ArrayToString<Vector3>(translationDirectionsPool));
    }

    /// <summary>Arguments should be given in seconds.</summary>
    public IEnumerator RandomBuildingTranslationLoop(float translationLoopDuration, int numEvents, float waitAtTheBeginning)
    {

        int eventNum = 1;
        float globalBegin = Time.time;
        float timePerEvent = (translationLoopDuration - waitAtTheBeginning) / numEvents;

        if (!ModeController.TutorialMode) {
            StartCoroutine(Utils.Timer(translationLoopDuration));
            
            Debug.Log("RandomBuildingTranslationLoop start Time.time: " + Time.time);
            Debug.Log("RandomBuildingTranslationLoop predicted end Time.time: " + (Time.time + translationLoopDuration));            
            
            Debug.Log("Waiting for: " + waitAtTheBeginning);
            yield return new WaitForSeconds(waitAtTheBeginning);
        }

        while (true)
        {
            yield return null;

            if (currentTranslationCoroutine == null && laserPointer.Ready && voxelDrawer.Ready)
            {
                if (ModeController.TutorialMode) 
                {
                    yield return new WaitForSeconds(2f);
                    currentTranslationCoroutine = StartCoroutine(RandomlyTranslateRandomBuilding());
                }
                else
                {
                    if (eventNum > numEvents) {
                        break;
                    }


                    // Figure out for how long to wait
                    float beginTime, endTime;
                    float eventLaunchTime = 0; // just to calm compiler down
                    for ( ; eventNum <= numEvents; ) {
                        beginTime = Time.time;
                        endTime = globalBegin + waitAtTheBeginning + eventNum * timePerEvent;

                        float randWeight = buildingTranslationTimeRandomVariableCurve.Evaluate(UnityEngine.Random.value);
                        eventLaunchTime = beginTime + (endTime - beginTime) * randWeight;//UnityEngine.Random.Range(beginTime, endTime);

                        Debug.Log("Event " + eventNum + " launch time: " + eventLaunchTime + ". Time.time: " + Time.time);
                    
                        while (Time.time < eventLaunchTime) {
                            float timeDifference = eventLaunchTime - Time.time;
                            if (timeDifference > 0) {
                                Debug.Log("Waiting for (eventLaunchTime - Time.time): " + timeDifference);
                                yield return new WaitForSeconds(Mathf.Abs(Time.time - eventLaunchTime));
                            } else {
                                break;
                            }
                        }
                    
                        // Tranlsate the building
                        currentTranslationCoroutine = StartCoroutine(RandomlyTranslateRandomBuilding());

                        eventNum++;
                        break;
                    }
                }

            }
        }

        Debug.Log("RandomBuildingTranslationLoop end Time.time: " + Time.time);
    }

    public IEnumerator RandomlyTranslateRandomBuilding()
    {
        Vector3 from;
        Vector3 to;

        GameObject randomBuilding = null;
        Vector3 actualBuildingCenter;
        Nullable<Vector3> randTranslationDestination;

        // Find the translation destination relative to soundSource render Bounds.center
        do {
            if (randomBuilding != null) {
                DestroyChildren(randomBuilding);
            }

            randomBuilding = SelectRandomBuilding();
            
            if (ModeController.SoundCues) {
                AttachSoundSource(randomBuilding);
            }
            AttachSphereCollider(randomBuilding);
            soundSource = randomBuilding;

            from = soundSource.transform.position;

            actualBuildingCenter = GetGameObjectCenterInScene(soundSource);
            
            randTranslationDestination = ChooseRandomTranslationDestination(actualBuildingCenter, randomBuildingTranslationLayer);
            
            // Debug.Log("randTranslationDestination: " + randTranslationDestination);

            if (randTranslationDestination == null) {
                to = from; // so the distance is 0 < 150f
                continue;
            }
            
            // Adjust for incorrect origins the buildings in the Campus model have
            to = randTranslationDestination.Value + from - GetGameObjectCenterInScene(soundSource); // rectangleCorners -> ComputeRandomTranslation(from)
            
            yield return null;
        } while (/* randTranslationDestination == null || */ Vector3.Distance(to, from) < 150f); // || Vector3.Distance(to, from) > 160f);

        Debug.DrawRay(actualBuildingCenter, randTranslationDestination.Value - actualBuildingCenter, Color.red, 5f);

        StartTranslateSoundSource(from, to);
    }

    private Nullable<Vector3> ChooseRandomTranslationDestination(Vector3 origin, int layerMask)
    {
        Nullable<Vector3> translationDestination = null;
        List<Vector3> translationDirectionsPoolList;
        Vector3 translationDirection;

        translationDirectionsPoolList = new List<Vector3>(translationDirectionsPool);

        // Debug rays
        /* foreach (var translationDir in translationDirectionsPoolList) {
			Debug.DrawRay(origin, translationDir * 1000f, Color.red, 10f);
		} */

        // Debug.Log("translationDirectionsPoolList.Count: " + translationDirectionsPoolList.Count);
        List<Vector3> raycastHitPoints = new List<Vector3>();
        while (translationDirectionsPoolList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, translationDirectionsPoolList.Count);
            translationDirection = translationDirectionsPoolList[randomIndex];
            Vector3 tempOrigin = origin;

            RaycastHit hit;
            while (Physics.Raycast(tempOrigin, translationDirection, out hit, 1000f, layerMask)
                || Physics.Raycast(tempOrigin + translationDirection, translationDirection, out hit, 1000f, layerMask) // just so it works for demo purposes when all buildings were already tranalated to the SphereTranslationCubeCollider
            ) // usually, the ray would intersect the wall in 2 places
            {
                // Debug.Log("translationDirection: " + translationDirection);

                if (hit.transform.tag == "Player")
                { // if we hit the sphere around the sphere, discard this direction vector
                    raycastHitPoints = new List<Vector3>();
                    break;
                }

                // Debug.DrawRay(tempOrigin, translationDirection * hit.distance, Color.red, 10f);

                tempOrigin = hit.point + translationDirection; // new origin == hit point + small offset, otherwise the the outward facing face is hit again (I think)

                // Debug.DrawRay(origin, translationDirection * 1000f, Color.red, 10f);

                raycastHitPoints.Add(hit.point);

                if (raycastHitPoints.Count >= 2)
                { // for cases when translation direction is parallel to a PathCube's side
                    break;
                }
            }

            if (raycastHitPoints.Count == 0)
            {
                translationDirectionsPoolList.RemoveAt(randomIndex);
            }
            else
            {
                // Debug.Log("Found " + raycastHitPoints.Count + " intersections");
                break;
            }
        }

        try {
            translationDestination = raycastHitPoints[UnityEngine.Random.Range(0, raycastHitPoints.Count)];
        } catch (ArgumentOutOfRangeException) {
            translationDestination = null;
        }

        return translationDestination;
    }

    private void AttachSoundSource(GameObject randomBuilding)
    {
        Instantiate(soundSourcePrefab, GetGameObjectCenterInScene(randomBuilding), Quaternion.identity, randomBuilding.transform);
    }

    public void AttachSphereCollider(GameObject randomBuilding)
    {
        Bounds randomBuildingRenderBounds = GetGameObjectRenderBounds(randomBuilding);

        GameObject collider = Instantiate(buildingColliderPrefab, randomBuildingRenderBounds.center, Quaternion.identity, randomBuilding.transform);

        // Change collider scale
        Vector3 newScale = Vector3.one *
            Mathf.Max(
                Vector3.Distance(randomBuildingRenderBounds.center, randomBuildingRenderBounds.max),
                Vector3.Distance(randomBuildingRenderBounds.center, randomBuildingRenderBounds.min
            )
        );
        collider.transform.localScale = newScale;

        // Change render layer
        int layerMaskEditor = (int)Mathf.Log(movingBuildingLayer.value, 2);
        collider.layer = layerMaskEditor;
        foreach (Transform t in collider.GetComponentInChildren<Transform>())
        {
            t.gameObject.layer = layerMaskEditor;
        }
    }

    public void DestroyChildren(GameObject randomBuilding)
    {
        int childCount = randomBuilding.transform.childCount;
        for (int i = childCount - 1; i > -1; i--)
        {
            Destroy(randomBuilding.transform.GetChild(i).gameObject);
        }
    }

    private Vector3 GetGameObjectCenterInScene(GameObject go)
    {
        return GetGameObjectRenderBounds(go).center;
    }

    private Bounds GetGameObjectRenderBounds(GameObject go)
    {
        return go.GetComponent<Renderer>().bounds;
    }

    /* public GameObject GetSoundSource() {
		return soundSource;
	} */

    private static Vector2 Vector3ToVector2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }

    private static Vector3 Vector2ToVector3(Vector2 v2)
    {
        return new Vector3(v2.x, 0f, v2.y);
    }

    private GameObject SelectRandomBuilding()
    {
        if (ModeController.TutorialMode) {
            return ModeController.TutorialBuilding;
        }
        else {
            return buildingsInCampus[UnityEngine.Random.Range(0, buildingsInCampus.Count)];
        }
    }

    public void PositionSoundSource(Vector3 at)
    {
        soundSource.transform.position = at;
    }

    public void StartTranslateSoundSource(Vector3 from, Vector3 to)
    {
        currentTranslationCoroutine = StartCoroutine(TranslateSoundSourceCoroutine(from, to));
    }

    public void StartTranslateSoundSource(Vector3 to)
    {
        StartTranslateSoundSource(soundSource.transform.position, to);
    }

    public void StopTranslation(bool destroyChildren)
    {
        if (currentTranslationCoroutine != null)
        {
            StopCoroutine(currentTranslationCoroutine);

            StopSound();

            if (destroyChildren)
            {
                DestroyChildren(soundSource);
            }
            
            currentTranslationCoroutine = null;
            Debug.Log("Finished TranslateSoundSource");
        }
    }

    private IEnumerator TranslateSoundSourceCoroutine(Vector3 from, Vector3 to)
    {
        Debug.Log("Started TranslateSoundSource"); //: distance == " + Vector3.Distance(from, to));

        Vector3 direction = (to - from).normalized;

        if (soundSource.transform.position != from)
        {
            PositionSoundSource(from);
        }

        PlaySound(); 

        // Store tranlation from
        // DataCollector.StoreTranslation(fromTransform, Time.unscaledTime);

        do 
        {
            soundSource.transform.position += direction * soundSourceSpeed * Time.deltaTime;
            yield return null;
        } while (Mathf.Round(Vector3.Distance(from, to) - Vector3.Distance(from, soundSource.transform.position)) > 0);

        soundSource.transform.position = to;

        StopTranslation(true);
    }

    private void PlaySound()
    {
        try {
            Debug.Log("#PlaySound");
            soundSource.transform.GetChild(0).GetComponent<AudioSource>().Play();
        } catch (Exception) { Debug.Log("Couldn't start sound! Possibly, sound cues are turned off."); }
    }

    private void StopSound()
    {
        try {
            Debug.Log("#StopSound");
            soundSource.transform.GetChild(0).GetComponent<AudioSource>().Stop();
        } catch (Exception) { Debug.Log("Couldn't stop sound! Possibly, sound cues are turned off."); }
    }

    public void RemoveFromBuildingList(GameObject building) {
        building.SetActive(false);
        buildingsInCampus.Remove(building);
    }
}
