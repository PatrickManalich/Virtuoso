using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBandScript : BandScript {

    public Material offMaterial;
    public Material onMaterial;
    private Animator animator;
    private new AnimationClip animation;
    private Renderer meshRenderer;
    private bool alive;
    private float lifetime;
    private bool toggle;


    private void Awake() {
        animator = GetComponent<Animator>();
        animation = animator.runtimeAnimatorController.animationClips[1];
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        meshRenderer.material = offMaterial;
        Vector3 parentPos = transform.parent.transform.position;
        transform.position = new Vector3(parentPos.x - 0.063f, parentPos.y - 0.025f, parentPos.z - 0.185f);
        transform.Rotate(-75, 90, 0);
        transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        alive = false;
        lifetime = 0.0f;
        toggle = false;
    }

    private IEnumerator CheckIfStillAlive() {
        if (Time.time - lifetime > animation.length) {
            animator.SetBool("Hovering", false);
            alive = false;
            yield return null;
        } else {
            yield return new WaitForSeconds(animation.length * (1 / animator.speed));
            yield return CheckIfStillAlive();
        }
    }

    public override void GiveLife() {
        if (!alive) {
            lifetime = Time.time;
            alive = true;
            animator.SetBool("Hovering", true);
            StartCoroutine(CheckIfStillAlive());
        }
    }

    public override void IncreaseLifetime() {
        lifetime = Time.time;
    }
    
    public override void Toggle() {
        if (toggle) {
            meshRenderer.material = offMaterial;
            toggle = false;
        } else {
            meshRenderer.material = onMaterial;
            toggle = true;
        }
    }

}
