using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBandScript : BandScript {

    private enum ToggleState { Off, On };   // The two state options are either off or on
    private ToggleState toggleState;        // The current state of the band
    private Renderer meshRenderer;          // The mesh renderer of the band
    private float toggleAnimationLength;    // The number of seconds the toggle animation lasts
    private GhostScript ghostScript;        // The ghost script of the ghost Game Object

    public Material offMaterial;            // The off material of the band
    public Material onMaterial;             // The on material of the band
    public GameObject ghost;                // The ghost Game Object

    private void Awake() {
            // Initialized private variables and set it to the fourth position on the wrist
        ghostScript = ghost.GetComponent<GhostScript>();
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = offMaterial;
        base.SetPosition(3);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
        toggleState = ToggleState.Off;
    }

    /* Changes the material of the toggle and either starts or stops the ghosting. */
    public override IEnumerator Toggle() {
        if (toggleState == ToggleState.Off) {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = onMaterial; // Changes material halfway through animation
            ghostScript.StartGhosting();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.On;
            yield return null;
        } else {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = offMaterial; // Changes material halfway through animation
            ghostScript.StopGhosting();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Off;
            yield return null;
        }
    }

}
