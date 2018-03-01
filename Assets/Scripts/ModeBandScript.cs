using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeBandScript : BandScript {

    private enum Mode { View, Edit };
    private Mode mode;
    private Animator animator;
    private new AnimationClip animation;
    private Renderer meshRenderer;
    //private PlayBandScript playBandScript;
    //private GhostBandScript ghostBandScript;
    private bool alive;
    private float lifetime;
    private bool toggle;

    public Material viewMaterial;
    public Material editMaterial;
    public GameObject playBand;
    public GameObject ghostBand;


    private void Awake() {
        animator = GetComponent<Animator>();
        animation = animator.runtimeAnimatorController.animationClips[1];
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        //playBandScript = playBand.GetComponent<PlayBandScript>();
        //ghostBandScript = ghostBand.GetComponent<GhostBandScript>();
        meshRenderer.material = viewMaterial;
        Vector3 parentPos = transform.parent.transform.position;
        transform.position = new Vector3(parentPos.x - 0.053f, parentPos.y, parentPos.z - 0.135f);
        transform.Rotate(-75, 90, 0);
        transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        alive = false;
        lifetime = 0.0f;
    }

    private void Start() {
        ghostBand.SetActive(false);
        mode = Mode.View;
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
        if (mode == Mode.View) {
            meshRenderer.material = editMaterial;
            playBand.SetActive(false);
            ghostBand.SetActive(true);
            mode = Mode.Edit;
        } else {
            meshRenderer.material = viewMaterial;
            ghostBand.SetActive(false);
            playBand.SetActive(true);
            mode = Mode.View;
        }
    }

}
