using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBandScript : BandScript {

    private enum ToggleState { RealTime, Fast, Slow };
    private ToggleState toggleState;
    private Renderer meshRenderer;
    private float toggleAnimationLength;

    public Material realTimeMaterial;
    public Material fastMaterial;
    public Material slowMaterial;


    private void Awake() {
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
            meshRenderer.material = fastMaterial;
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Fast;
            yield return null;
        } else if(toggleState == ToggleState.Fast) {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = slowMaterial;
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Slow;
            yield return null;
        } else {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = realTimeMaterial;
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.RealTime;
            yield return null;
        }
    }
}
