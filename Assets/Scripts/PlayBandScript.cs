using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBandScript : BandScript {

    private DummyManagerScript DMS;             // The dummy manager script
    private enum ToggleState { Pause, Play };   // The two state options are either pause or play
    private ToggleState toggleState;            // The current state of the band
    private Renderer meshRenderer;              // The mesh renderer of the band
    private float toggleAnimationLength;        // The number of seconds the toggle animation lasts

    public GameObject dummyManager;             // The dummy manager game object
    public Material pauseMaterial;              // The pause material of the band
    public Material playMaterial;               // The play material of the band

    private void Awake() {
            // Initialized private variables and set it to the third position on the wrist
        DMS = dummyManager.GetComponent<DummyManagerScript>();
        base.InitializeBand();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = pauseMaterial;
        base.SetPosition(2);
        toggleAnimationLength = GetComponent<Animator>().runtimeAnimatorController.animationClips[2].length;
        toggleState = ToggleState.Pause;
    }

    /* Changes the material of the toggle and either plays or pauses the dummy. */
    public override IEnumerator Toggle() {
        if (toggleState == ToggleState.Pause) {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = playMaterial; // Changes material halfway through animation
            DMS.DS_StartPlaying();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Play;
            yield return null;
        } else {
            base.TriggerToggled();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            meshRenderer.material = pauseMaterial; // Changes material halfway through animation
            DMS.DS_StopPlaying();
            yield return new WaitForSeconds(toggleAnimationLength / 2);
            toggleState = ToggleState.Pause;
            yield return null;
        }
    }

    /* Forces the play band to enter the pause toggle state. */
    public void ForcePauseToggle() {
        meshRenderer.material = pauseMaterial;
        toggleState = ToggleState.Pause;
    }
}
