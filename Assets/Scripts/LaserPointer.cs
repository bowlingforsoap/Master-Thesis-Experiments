using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DataCollector))]
public class LaserPointer : MonoBehaviour {

	public AudioSource selectionClip;
	public AudioSource deselectionClip;
	public Material neutralMaterial;
	public Material focusMaterial;
	public Material selectedMaterial;
	[SerializeField]
	private GameObject selectedPole1 = null;
	[SerializeField]
	private GameObject selectedPole2 = null;
	private bool firstPoleSelected = false;

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
				} else {
					hitPoint = hit.point;
					hitDistance = hit.distance;
				}

				ShowLaser();
			} else {
				HideLaser();
			}
			
			// Show laser
			if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) // Joystick press
			// if (Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger)) // Hair Trigger press
			{
				if (hitSomething)
				{
					Debug.Log("Found a pole!");

					if ((!firstPoleSelected && (selectedPole2 == null || (selectedPole2 !=null && hit.transform.gameObject.GetInstanceID() != selectedPole2.GetInstanceID()))) || // if the first pole is not selected && the hit is not the same as the selectedPole2
						hit.transform.gameObject.GetInstanceID() == selectedPole1.GetInstanceID()) { // or if it is, but we are raycasting the same pole
							firstPoleSelected = SelectFirstPole(hit);
					} else {
						// if (hit.transform.gameObject.GetInstanceID() != selectedPole1.GetInstanceID()) { // Quick fix
							SelectSecondPole(hit);
						// } else {
							// firstPoleSelected = SelectFirstPole(hit);
						// }
					}
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

	private void HideLaser() {
		laser.SetActive(false);
	}

	// Returns true if a pole was selected, false - otherwise.
	private bool SelectPole(RaycastHit hit, ref GameObject selectedPole) {
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
		selectedPole.GetComponent<Renderer>().material = selectedMaterial;
		return true;
	}

	private bool SelectFirstPole(RaycastHit hit) {
		return SelectPole(hit, ref selectedPole1);
	}

	private void SelectSecondPole(RaycastHit hit) {
		bool secondPoleSelected = SelectPole(hit, ref selectedPole2);
		if (secondPoleSelected) {
			DataCollector.LogGuess(selectedPole1.transform, selectedPole2.transform);
		}
	}

}
