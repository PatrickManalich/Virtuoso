using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBandScript : MonoBehaviour {

    private Animator animator;

	void Awake () {
        Vector3 parentPos = transform.parent.transform.position;
        transform.position = new Vector3(parentPos.x - 0.05f, parentPos.y - 0.02f, parentPos.z - 0.15f);
        transform.Rotate(0, 0, 270);
        animator = GetComponent<Animator>();
	}
	
    public void Hovering() {
        animator.SetTrigger("Hovering");
    }

    public void Unhovering() {
        animator.SetTrigger("Hovering");
    }


}
