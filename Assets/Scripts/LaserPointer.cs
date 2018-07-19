using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DataCollector))]
public class LaserPointer : MonoBehaviour {

	public GameObject rectilePrefab;
	[SerializeField]
	private GameObject rectile;
	private bool allowSelection = true;


	private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public GameObject laserPrefab;
    [SerializeField]
	private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;
	[SerializeField]
	private float hitDistance;
	private const float maxHitDistance = 200f;
    public LayerMask raycastMask;

    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;

		rectile = Instantiate(rectilePrefab, Vector3.zero, Quaternion.identity);
		rectile.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
			
			RaycastHit hit;
			bool hitSomething = Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, maxHitDistance, raycastMask);
			
			// Show laser
			if (Controller.GetTouch(SteamVR_Controller.ButtonMask.Axis0)) // Joystick touch
			{
				
				if (!hitSomething)
				{
					hitPoint = transform.position + transform.forward * maxHitDistance;
					hitDistance = maxHitDistance;
					
					HideRectile();
				} else {
					hitPoint = hit.point;
					hitDistance = hit.distance;
					
					ShowRectile(hit.point);
				}

				ShowLaser();
			} else {
				HideLaser();
				HideRectile();
			}
			

			if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) // Joystick press
			// if (Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger)) // Hair Trigger press
			{
				if (hitSomething)
				{
					DataCollector.StoreGuess(hit.point, Time.unscaledTime);
				}
			}
				

			// Select ppole
			// if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
				
			// }
	}

    private void ShowLaser()
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f); // Place the laser in the middle between the controller and the hitPoint
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hitDistance);
    }

	private void ShowRectile(Vector3 position) {
		rectile.transform.position = position;
		rectile.SetActive(true);
	}

	private void HideRectile() {
		rectile.SetActive(false);
	}

	private void HideLaser() {
		laser.SetActive(false);
	}

	// Returns true if a pole was selected, false - otherwise.
	/* private bool SelectPole(RaycastHit hit, ref GameObject selectedPole, ref Material selectedPoleMaterial) {
		if (!allowSelection) {
			Debug.Log("Processing previous guess. Selection not allowed!");
			return false;
		}

		// Deselect old
		if (selectedPole != null) {
			selectedPole.GetComponent<Renderer>().material = neutralMaterial;
			if (selectedPole.GetInstanceID() == hit.transform.gameObject.GetInstanceID()) { // We selected the same one
				deselectionClip.Play();
				selectedPole = null;
				return false;
			}
		}

		// Select new
		selectionClip.Play();
		selectedPole = hit.transform.gameObject;
		selectedPole.GetComponent<Renderer>().material = selectedPoleMaterial;
		return true;
	}

	private bool SelectFirstPole(RaycastHit hit) {
		return SelectPole(hit, ref selectedPole1, ref selectedMaterial1);
	}

	private void SelectSecondPole(RaycastHit hit) {
		bool secondPoleSelected = SelectPole(hit, ref selectedPole2, ref selectedMaterial2);
		if (secondPoleSelected) {
			DataCollector.LogGuess(selectedPole1.transform, selectedPole2.transform);
			StartCoroutine(DeselectedPoles());
		}
	} */

	/* private IEnumerator DeselectedPoles() {
		allowSelection = false;
		
		yield return new WaitForSeconds(.5f);
		selectedPole2.GetComponent<Renderer>().material = neutralMaterial;
		selectedPole2 = null;
		selectedPole1.GetComponent<Renderer>().material = neutralMaterial;
		selectedPole1 = null;
		firstPoleSelected = false;
		
		allowSelection = true;
	} */

}
