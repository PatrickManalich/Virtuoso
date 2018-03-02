using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBandScript : BandScript {

    private enum ToggleState { Off, On };
    private ToggleState toggleState;
    private Renderer meshRenderer;
    private float toggleAnimationLength;

    public Material offMaterial;
    public Material onMaterial;


    private void Awake() {
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = offMaterial;
        base.SetPosition(1);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
    }

    private void Start() {
        toggleState = ToggleState.Off;
    }

    public override IEnumerator Toggle() {
        if (toggleState == ToggleState.Off) {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = onMaterial;
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.On;
            yield return null;
        } else {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = offMaterial;
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Off;
            yield return null;
        }
    }

}
