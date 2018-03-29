using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoBandScript : BandScript {
    
    private Renderer meshRenderer;          // The mesh renderer of the band

    public Material undoMaterial;           // The start material of the band


    private void Awake() {
            // Initialized private variables and set it to the fourth position on the wrist
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = undoMaterial;
        base.SetPosition(4);
    }

    /**/
    public override IEnumerator Toggle() {
        base.TriggerToggled();
        yield return null;
    }
}
