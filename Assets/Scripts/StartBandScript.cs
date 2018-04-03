using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBandScript : BandScript {

    private Renderer meshRenderer;          // The mesh renderer of the band
    private DummyScript dummyScript;        // The dummy script of the dummy Game Object

    public Material startMaterial;          // The start material of the band
    public GameObject dummy;                // The dummy Game Object


    private void Awake() {
            // Initialized private variables and set it to the second position on the wrist
        dummyScript = dummy.GetComponent<DummyScript>();
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = startMaterial;
        base.SetPosition(1);
    }

    /* */
    public override IEnumerator Toggle() {
        base.TriggerToggled();
        dummyScript.GoToStart();
        yield return null;
    }
}
