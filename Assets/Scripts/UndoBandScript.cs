﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoBandScript : BandScript {

    private enum ToggleState { Undo, Redo };   // The two state options are either undo or redo
    private ToggleState toggleState;        // The current state of the band
    private Renderer meshRenderer;          // The mesh renderer of the band
    private float toggleAnimationLength;    // The number of seconds the toggle animation lasts

    public Material undoMaterial;           // The undo material of the band
    public Material redoMaterial;           // The redo material of the band

    private void Awake() {
        // Initialized private variables and set it to the fifth position on the wrist
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = undoMaterial;
        base.SetPosition(4);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
        toggleState = ToggleState.Undo;
    }

    /* Changes the material of the toggle and either undo or redo. */
    public override IEnumerator Toggle() {
        if (toggleState == ToggleState.Undo) {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = redoMaterial; // Changes material halfway through animation
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Redo;
            yield return null;
        } else {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = undoMaterial; // Changes material halfway through animation
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Undo;
            yield return null;
        }
    }

}