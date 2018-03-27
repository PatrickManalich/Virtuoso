using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBandScript : BandScript {

    private enum ToggleState { RealTime, Fast, Slow };  // The three state options are realtime, fast, or slow
    private ToggleState toggleState;                    // The current state of the band
    private Renderer meshRenderer;                      // The mesh renderer of the band
    private float toggleAnimationLength;                // The number of seconds the toggle animation lasts

    public Material realTimeMaterial;                   // The realtime material of the band
    public Material fastMaterial;                       // The fast material of the band
    public Material slowMaterial;                       // The slow material of the band

    private void Awake() {
            // Initialized private variables and set it to the first position on the wrist
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = realTimeMaterial;
        base.SetPosition(0);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
    }

    private void Start() {
        toggleState = ToggleState.RealTime;
    }

    public override IEnumerator Toggle() {
        if (toggleState == ToggleState.RealTime) {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = fastMaterial; // Changes material halfway through animation
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Fast;
            yield return null;
        } else if(toggleState == ToggleState.Fast) {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = slowMaterial; // Changes material halfway through animation
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Slow;
            yield return null;
        } else {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = realTimeMaterial; // Changes material halfway through animation
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.RealTime;
            yield return null;
        }
    }
}
