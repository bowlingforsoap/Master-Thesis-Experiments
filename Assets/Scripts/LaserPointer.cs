using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserPointer : MonoBehaviour {
	public Material neutralMaterial;
	public Material focusMaterial;
	public Material selectedMaterial;
	private GameObject poleSelected = null;

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

					// Deselect old
					if (poleSelected != null) {
						poleSelected.GetComponent<Renderer>().material = neutralMaterial;
						if (poleSelected.GetInstanceID() == hit.transform.gameObject.GetInstanceID()) { // We selected the same one
							poleSelected = null;
							return;
						}
					}

					// Select new
					poleSelected = hit.transform.gameObject;
					poleSelected.GetComponent<Renderer>().material = selectedMaterial;
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
}
