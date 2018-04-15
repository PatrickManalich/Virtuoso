using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineGuideScript : MonoBehaviour {

    private bool isGuiding;                         // If true, the refine guide is currently guiding
    private Vector3 anchorPoint;                    // Original position before refinement
    private const float positionOffset = 4.0f;      // Sets the position off to allow for clean pulling
    private const float lengthOffset = 20.0f;       // Sets the length off to allow for clean pulling

    public GameObject dummy;                        // The dummy Game Object

    private void Awake() {
        isGuiding = false;
        gameObject.SetActive(false);
    }

    private void Update() {
        if (isGuiding) {    // If it is guiding, adjust its position, rotation and scale based on dummy's position
            Vector3 newPosition = (dummy.transform.position - anchorPoint) / positionOffset + anchorPoint;
            Quaternion newRotation = Quaternion.FromToRotation(Vector3.up, (dummy.transform.position - anchorPoint));
            float newLength = Vector3.Magnitude(dummy.transform.position - anchorPoint) * lengthOffset;
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
