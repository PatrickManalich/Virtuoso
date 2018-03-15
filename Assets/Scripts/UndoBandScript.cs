using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoBandScript : BandScript {
    
    private Renderer meshRenderer;
    private float toggleAnimationLength;

    public Material undoMaterial;


    private void Awake() {
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = undoMaterial;
        base.SetPosition(4);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
    }


    public override IEnumerator Toggle() {
        base.TriggerToggled();
        yield return null;
    }
}
