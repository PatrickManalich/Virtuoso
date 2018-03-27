using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BandScript : MonoBehaviour {

    private Animator animator;  // The animator that every band have

    protected void InitializeBand() {
        animator = GetComponent<Animator>();
    }

    /* Takes in a band position which represents in which order it should be positioned on the wrist. For 
     * example, a band position of 0 will be closest to the wrist, a band position of 1 will be placed
     * beside the first band, etc. */
    protected void SetPosition(int bandPosition) {
        float bandOffset = bandPosition * 0.035f + 0.135f;
        transform.localPosition = new Vector3(-0.073f, 0f, -bandOffset);
        transform.Rotate(-75, 90, 0);
        transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
    }

    // Triggers the toggled trigger in the animator
    protected void TriggerToggled() {
        animator.SetTrigger("Toggled");
    }

    // Sets the hovering bool in the animator to true
    public void Hovering() {
        animator.SetBool("Hovering", true);
    }

    // Sets the hovering bool in the animator to false
    public void Unhovering() {
        animator.SetBool("Hovering", false);
    }

    // All bands are required to implement a function that handles what happens when they are toggled
    public abstract IEnumerator Toggle();
}
