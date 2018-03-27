using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBandScript : BandScript {

    private Renderer meshRenderer;          // The mesh renderer of the band
    private float toggleAnimationLength;    // The number of seconds the toggle animation lasts

    public Material startMaterial;          // The start material of the band


    private void Awake() {
            // Initialized private variables and set it to the second position on the wrist
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = startMaterial;
        base.SetPosition(1);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
    }


    public override IEnumerator Toggle() {
        base.TriggerToggled();
        yield return null;
    }
}
