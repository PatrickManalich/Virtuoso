using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BandScript : MonoBehaviour {

    private Animator animator;

    protected void InitializeBand() {
        animator = GetComponent<Animator>();
    }

    protected void SetPosition(int bandPosition) {
        float bandOffset = bandPosition * 0.035f + 0.135f;
        Vector3 parentPos = transform.parent.transform.position;
        transform.position = new Vector3(parentPos.x - 0.053f, parentPos.y, parentPos.z - bandOffset);
        transform.Rotate(-75, 90, 0);
        transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
    }

    protected void TriggerToggled() {
        animator.SetTrigger("Toggled");
    }

    public void Hovering() {
        animator.SetBool("Hovering", true);
    }

    public void Unhovering() {
        animator.SetBool("Hovering", false);
    }

    public abstract IEnumerator Toggle();
}
