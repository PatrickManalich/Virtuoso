using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBandScript : BandScript {

    private enum ToggleState { Pause, Play };
    private ToggleState toggleState;
    private Renderer meshRenderer;
    private float toggleAnimationLength;

    public Material pauseMaterial;
    public Material playMaterial;

    private void Awake() {
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = pauseMaterial;
        base.SetPosition(2);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
    }

    private void Start() {
        toggleState = ToggleState.Pause;
    }

    public override IEnumerator Toggle() {
        if (toggleState == ToggleState.Pause) {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = playMaterial;
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Play;
            yield return null;
        } else {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = pauseMaterial;
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Pause;
            yield return null;
        }
    }

}
