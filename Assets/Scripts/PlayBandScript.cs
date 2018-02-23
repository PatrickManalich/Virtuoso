using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBandScript : BandScript {

    private Animator animator;
    private bool toggle;
    public Material playMaterial;
    public Material pauseMaterial;
    private GameObject mesh;

    void Awake() {
        Vector3 parentPos = transform.parent.transform.position;
        transform.position = new Vector3(parentPos.x - 0.063f, parentPos.y - 0.025f, parentPos.z - 0.135f);
        transform.Rotate(-75, 90, 0);
        transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        animator = GetComponent<Animator>();
        toggle = false;
        mesh = transform.GetChild(0).gameObject;
        mesh.GetComponent<SkinnedMeshRenderer>().material = pauseMaterial;
        Debug.Log(mesh.GetComponent<SkinnedMeshRenderer>().material.name);
    }

    public override void Hovering() {
        animator.SetTrigger("Hovering");
    }

    public override void Unhovering() {
        animator.SetTrigger("Hovering");
    }

    public override void Toggle() {
        if (toggle) {
            mesh.GetComponent<SkinnedMeshRenderer>().material = pauseMaterial;
            toggle = false;
        } else {
            mesh.GetComponent<SkinnedMeshRenderer>().material = playMaterial;
            toggle = true;
        }

    }


}
