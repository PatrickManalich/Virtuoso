using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBandScript : BandScript {

    private Renderer meshRenderer;          // The mesh renderer of the band

    public Material startMaterial;          // The start material of the band


    private void Awake() {
            // Initialized private variables and set it to the second position on the wrist
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = startMaterial;
        base.SetPosition(1);
    }

    /**/
    public override IEnumerator Toggle() {
        base.TriggerToggled();
        yield return null;
    }
}
