using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineGuideScript : MonoBehaviour {

    private DummyManagerScript DMS;                 // The dummy manager script
    private bool isGuiding;                         // If true, the refine guide is currently guiding
    private Vector3 anchorPoint;                    // Original position before refinement
    private const float positionOffset = 4.0f;      // Sets the position off to allow for clean pulling
    private const float lengthOffset = 20.0f;       // Sets the length off to allow for clean pulling

    public GameObject dummyManager;                 // The dummy manager game object

    private void Awake() {
            // Initializes DMS, and sets it inactive and not guiding initially
        DMS = dummyManager.GetComponent<DummyManagerScript>();
        isGuiding = false;
        gameObject.SetActive(false);
    }

    private void Update() {
        if (isGuiding) {    // If it is guiding, adjust its position, rotation and scale based on dummy's position
            Vector3 newPosition = (DMS.GetSelectedDummy().transform.position - anchorPoint) / positionOffset + anchorPoint;
            Quaternion newRotation = Quaternion.FromToRotation(Vector3.up, (DMS.GetSelectedDummy().transform.position - anchorPoint));
            float newLength = Vector3.Magnitude(DMS.GetSelectedDummy().transform.position - anchorPoint) * lengthOffset;
            transform.position = newPosition;
            transform.rotation = newRotation;
            transform.localScale = V3E.SetY(transform.localScale, newLength);
        }
    }

    /* Takes in a new anchor point and begins guiding using this new anchor point. */
    public void StartGuiding(Vector3 newAnchorPoint) {
        gameObject.SetActive(true);
        anchorPoint = newAnchorPoint;
        transform.position = anchorPoint;
        isGuiding = true;
    }

    /* Stops guiding the dummy refinement */
    public void StopGuiding() {
        isGuiding = false;
        gameObject.SetActive(false);
    }
}
