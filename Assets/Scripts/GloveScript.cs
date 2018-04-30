using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveScript : MonoBehaviour {

    public enum GloveType { Left, Right };  // The two type of gloves are either left or right, used for positioning
    private enum ToggleState { Overwrite, Refine };   // The two state options are either overwrite or refine
    private ToggleState toggleState;        // The current state of the band
    private Renderer meshRenderer;          // The mesh renderer of the band

    public GloveType gloveType;             // Used to specify whether it is the left glove or right glove
    public Material overwriteMaterial;      // The overwrite material of the band
    public Material refineMaterial;         // The refine material of the band

    private void Awake() {
            // Sets its position and initializes private variables
        if(gloveType == GloveType.Left) {
            transform.localPosition = new Vector3(-0.063f, -0.031f, -0.053f);
            transform.Rotate(0f, 0f, -90f);            
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        } else {
            transform.localPosition = new Vector3(0.063f, -0.031f, -0.053f);
            transform.Rotate(0f, 0f, 90);
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        meshRenderer = GetComponent<Renderer>();
        meshRenderer.material = overwriteMaterial;
        toggleState = ToggleState.Overwrite;
    }

    private void Update() {
        if (OVRInput.GetDown(OVRInput.Button.Four) || OVRInput.GetDown(OVRInput.Button.Two)) {
            if (toggleState == ToggleState.Overwrite) {
                meshRenderer.material = refineMaterial;
                toggleState = ToggleState.Refine;
            } else {
                meshRenderer.material = overwriteMaterial;
                toggleState = ToggleState.Overwrite;
            }
        }
    }

    /* Returns true if in overwrite state, false otherwise. */
    public bool isInOverwriteState() {
        if (toggleState == ToggleState.Overwrite)
            return true;
        else
            return false;
    }
}
