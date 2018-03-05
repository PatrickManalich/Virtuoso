using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeBandScript : BandScript {

    private enum ToggleState { View, Edit };
    private ToggleState toggleState;
    private Renderer meshRenderer;
    private float toggleAnimationLength;

    public Material viewMaterial;
    public Material editMaterial;
    public GameObject endBand;
    public GameObject playBand;
    public GameObject startBand;
    public GameObject ghostBand;
    public GameObject undoBand;


    private void Awake() {
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = viewMaterial;
        base.SetPosition(0);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
    }

    private void Start() {
        ghostBand.SetActive(false);
        undoBand.SetActive(false);
        toggleState = ToggleState.View;
    }

    public override IEnumerator Toggle() {
        if (toggleState == ToggleState.View) {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);

            meshRenderer.material = editMaterial;
            endBand.SetActive(false);
            playBand.SetActive(false);
            startBand.SetActive(false);
            ghostBand.SetActive(true);
            undoBand.SetActive(true);

            yield return new WaitForSeconds(toggleAnimationLength / 2);

            toggleState = ToggleState.Edit;
            yield return null;
        } else {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);

            meshRenderer.material = viewMaterial;
            ghostBand.SetActive(false);
            undoBand.SetActive(false);
            endBand.SetActive(true);
            playBand.SetActive(true);
            startBand.SetActive(true);

            yield return new WaitForSeconds(toggleAnimationLength / 2);

            toggleState = ToggleState.View;
            yield return null;
        }
    }

    


}
