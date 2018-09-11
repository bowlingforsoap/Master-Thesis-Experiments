using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    public GameObject tutorialBuilding;
	public bool soundCues;
	public bool minimap;

	public static GameObject TutorialBuilding {
		get {
			return instance.tutorialBuilding;
		}
	}
	public static bool TutorialMode {
		get {
			return instance.tutorialMode;
		}
	}
	public static bool SoundCues {
		get {
			return instance.soundCues;
		}
	}
	public static bool Minimap {
		get {
			return instance.minimap;
		}
	}

    private GameObject campus;
	private bool tutorialMode = false;

	private static ModeController instance;

	void Awake() {
		instance = this;
	}

    // Use this for initialization
    void Start()
    {
        // GoInTutorialMode();
    }

    public void GoInTutorialMode()
    {
		tutorialMode = true;

        Utils.SetGameObjectsActive(SoundSourceTranslationController.BuildingsInCampus, false);
        tutorialBuilding.SetActive(true);
    }

    public void GoInExperimentMode()
    {
		tutorialMode = false;
		
		Utils.SetGameObjectsActive(SoundSourceTranslationController.BuildingsInCampus, true);
    }

	public void GoInMinimapOnlyMode() {
		Debug.Log("Minimap only mode activated!");
		minimap = true;
		soundCues = false;
	}
	public void GoInSoundOnlyMode() {
		Debug.Log("Sound only mode activated!");
		minimap = false;
		soundCues = true;
	}
	public void GoInMinimapAndSoundMode() {
		Debug.Log("Minimap + Sound mode activated!");
		minimap = true;
		soundCues = true;
	}
}
