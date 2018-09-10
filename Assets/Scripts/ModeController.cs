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
}
