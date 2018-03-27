using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoBandScript : BandScript {
    
    private Renderer meshRenderer;          // The mesh renderer of the band
    private float toggleAnimationLength;    // The number of seconds the toggle animation lasts

    public Material undoMaterial;           // The start material of the band


    private void Awake() {
            // Initialized private variables and set it to the fourth position on the wrist
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
