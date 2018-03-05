using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBandScript : BandScript {

    private Renderer meshRenderer;
    private float toggleAnimationLength;

    public Material startMaterial;


    private void Awake() {
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = startMaterial;
        base.SetPosition(3);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
    }


    public override IEnumerator Toggle() {
        base.TriggerToggled();
        yield return null;
    }
}
