using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBandScript : BandScript {

    private DummyManagerScript DMS;
    private Renderer meshRenderer;          // The mesh renderer of the band

    public GameObject dummyManager;         // The dummy manager game object
    public Material startMaterial;          // The start material of the band


    private void Awake() {
            // Initialized private variables and set it to the second position on the wrist
        DMS = dummyManager.GetComponent<DummyManagerScript>();
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = startMaterial;
        base.SetPosition(1);
    }

    /* */
    public override IEnumerator Toggle() {
        base.TriggerToggled();
        DMS.DS_GoToStart();
        yield return null;
    }
}
