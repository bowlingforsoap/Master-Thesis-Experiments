using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundSourceTranslationController : MonoBehaviour
{
    public float soundSourceSpeed;
    public Transform center;
    public Transform leftFront, rightFront, leftBack, rightBack;//, left, right, front, back;
    public GameObject soundSourcePrefab;
	public GameObject sphereCollider;
    public GameObject campus;
    public LayerMask randomBuildingTranslationLayer;

    [SerializeField]
    private GameObject soundSource;
    [SerializeField]
    private Coroutine currentCoroutine;
    // [SerializeField]
    // private Vector2 rectangleSize;
    // [SerializeField]
    // private Vector2[] rectangleCorners = new Vector2[4];
    [SerializeField]
    private List<GameObject> buildingsInCampus;
    private Vector3[] translationDirectionsPool;


    void Start()
    {
        /* Debugger.InstantiateHierarchyDelimiter("-------------");
        Debugger.InstantiateEmptyAt(leftFront.position, "lf");
        Debugger.InstantiateEmptyAt(rightFront.position, "rf");
        Debugger.InstantiateEmptyAt(rightBack.position, "rb");
        Debugger.InstantiateEmptyAt(leftBack.position, "lb");
        Debugger.InstantiateEmptyAt(leftFront.position, "lf");
        Debugger.ConnectIntantiatedGameObjects(true); */

        // Find all buildings in campus model
        int modelsInCampus = campus.transform.childCount;
        for (int i = 0; i < modelsInCampus; i++)
        {
            Transform model = campus.transform.GetChild(i);

            if (model.name.Contains("Text") || model.name.Contains("Bridge") || model.name.Contains("Subregion") || model.name.Contains("Surface"))
            {
                continue;
            }

            if (model.gameObject.active)
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

    public void RandomlyTranslateRandomBuilding()
    {
        Vector3 from;
        Vector3 to;

        GameObject randomBuilding = SelectRandomBuilding();

        AttachSoundSource(randomBuilding);
        soundSource = randomBuilding;

        from = soundSource.transform.position;

        Vector3 actualBuildingCenter = GetGameObjectCenterInScene(soundSource);
        Debugger.InstantiateHierarchyDelimiter("------------");
        Debugger.InstantiateEmptyAt(actualBuildingCenter, "building");
        Nullable<Vector3> randTranslationDestination = ChooseRandomTranslationDestination(actualBuildingCenter, randomBuildingTranslationLayer);
        // Adjust for incorrect origins the buildings in the Campus model have
        to = randTranslationDestination.Value + from - GetGameObjectCenterInScene(soundSource); // rectangleCorners -> ComputeRandomTranslation(from)

        /* // Debug.Log("Sound source center: " + soundSourceCenter2D);
		Debug.Log("Rectangle corner: " + rectangleCorners[0]);
		Debug.Log("Actually translating to: " + to);
		
		Debugger.InstantiateAt(Vector3.zero, "------------");
		// Debugger.InstantiateAt(Vector2ToVector3(soundSourceCenter2D), "soundSource");
		Debugger.InstantiateAt(center.position, "center");
		Debugger.InstantiateAt(Vector2ToVector3(rectangleCorners[0]), "rectangleCorners[0]");
		Debugger.InstantiateAt(to, "final to");

		Debugger.ConnectIntantiatedGameObjects(); */

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

        Debug.Log("translationDirectionsPoolList.Count: " + translationDirectionsPoolList.Count);
        List<Vector3> raycastHitPoints = new List<Vector3>();
        while (translationDirectionsPoolList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, translationDirectionsPoolList.Count);
            translationDirection = translationDirectionsPoolList[randomIndex];
			Vector3 tempOrigin = origin;

			RaycastHit hit;
            while (Physics.Raycast(tempOrigin, translationDirection, out hit, 1000f, layerMask)) // usually, the ray would intersect the wall in 2 places
            {
				Debug.Log("translationDirection: " + translationDirection);

				if (hit.transform.tag == "Player") { // if we hit the sphere around the sphere, discard this direction vector
					raycastHitPoints = new List<Vector3>();
					break;
				}

                Debug.DrawRay(tempOrigin, translationDirection * hit.distance, Color.red, 10f);

                tempOrigin = hit.point + translationDirection; // new origin == hit point + small offset, otherwise the the outward facing face is hit again (I think)

                // Debug.DrawRay(origin, translationDirection * 1000f, Color.red, 10f);

                raycastHitPoints.Add(hit.point);

				if (raycastHitPoints.Count >= 2) { // for cases when translation direction is parallel to a PathCube's side
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

        translationDestination = raycastHitPoints[UnityEngine.Random.Range(0, raycastHitPoints.Count)];

        return translationDestination;
    }

    private void AttachSoundSource(GameObject randomBuilding)
    {
        Instantiate(soundSourcePrefab, GetGameObjectCenterInScene(randomBuilding), Quaternion.identity, randomBuilding.transform);
    }

	private void AttachSphereCollider(GameObject randomBuilding) 
	{
		Instantiate(sphereCollider, GetGameObjectCenterInScene(randomBuilding), Quaternion.identity, randomBuilding.transform);
	}

	public void DestroyChildren(GameObject randomBuilding) {
		int childCount = randomBuilding.transform.childCount;
		for (int i = childCount - 1; i > -1; i--) {
			Destroy(randomBuilding.transform.GetChild(i).gameObject);
		}
	}

    private Vector3 GetGameObjectCenterInScene(GameObject go)
    {
        return go.GetComponent<Renderer>().bounds.center;
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
        return buildingsInCampus[UnityEngine.Random.Range(0, buildingsInCampus.Count)];
    }

    public void PositionSoundSource(Vector3 at)
    {
        soundSource.transform.position = at;
    }

    public void StartTranslateSoundSource(Vector3 from, Vector3 to)
    {
        currentCoroutine = StartCoroutine(TranslateSoundSourceCoroutine(from, to));
    }

    public void StartTranslateSoundSource(Vector3 to)
    {
        StartTranslateSoundSource(soundSource.transform.position, to);
    }

    public void StopTranslation()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);

            StopSound();

			DestroyChildren(soundSource);
        }
    }

    private IEnumerator TranslateSoundSourceCoroutine(Vector3 from, Vector3 to)
    {
        Debug.Log("Started TranslateSoundSource");

        Vector3 direction = (to - from).normalized;

        if (soundSource.transform.position != from)
        {
            PositionSoundSource(from);
        }

        // TODO: add AudioClip to VB on the fly
        // PlaySound(); 

        // Store tranlation from
        // DataCollector.StoreTranslation(fromTransform, Time.unscaledTime);

        while (Vector3.Distance(from, to) > Vector3.Distance(from, soundSource.transform.position))
        {
            soundSource.transform.position += direction * soundSourceSpeed * Time.deltaTime;
            yield return null;
        }
        soundSource.transform.position = to;

        // StopSound();
		
		DestroyChildren(soundSource);

        currentCoroutine = null;
        Debug.Log("Finished TranslateSoundSource");
    }

    private void PlaySound()
    {
        soundSource.transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

    private void StopSound()
    {
        soundSource.transform.GetChild(0).GetComponent<AudioSource>().Stop();
    }
}
