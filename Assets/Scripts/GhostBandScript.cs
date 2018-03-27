﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBandScript : BandScript {

    private enum ToggleState { Off, On };   // The two state options are either off or on
    private ToggleState toggleState;        // The current state of the band
    private Renderer meshRenderer;          // The mesh renderer of the band
    private float toggleAnimationLength;    // The number of seconds the toggle animation lasts

    public Material offMaterial;            // The off material of the band
    public Material onMaterial;             // The on material of the band


    private void Awake() {
            // Initialized private variables and set it to the fourth position on the wrist
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = offMaterial;
        base.SetPosition(3);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
    }

    private void Start() {
        toggleState = ToggleState.Off;
    }

    public override IEnumerator Toggle() {
        if (toggleState == ToggleState.Off) {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = onMaterial; // Changes material halfway through animation
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.On;
            yield return null;
        } else {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = offMaterial; // Changes material halfway through animation
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Off;
            yield return null;
        }
    }

}
