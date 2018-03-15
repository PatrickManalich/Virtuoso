using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderFieldScript : MonoBehaviour {

    private float halfLength;
    private Transform compoundColliderTransform;
    private GameObject sliderStart;
    private GameObject sliderCurrent;
    private GameObject sliderEnd;

    public Material sliderIdleMaterial;
    public Material sliderHoveringMaterial;
    public Material sliderSlidingMaterial;

	private void Awake() {
        transform.localPosition = new Vector3(-0.086f, -0.055f, -0.2047f);
        transform.Rotate(-75, 90, 0);
        transform.localScale = new Vector3(0.82f, 0.82f, 0.82f);

        compoundColliderTransform = transform.GetChild(0);
        compoundColliderTransform.localPosition = new Vector3(0.0f, 0.03f, -0.005f);
        compoundColliderTransform.localScale = new Vector3(0.215f, 0.09f, 0.06f);

        halfLength = 0.1f;
        sliderStart = transform.GetChild(1).gameObject;
        sliderStart.transform.localPosition = new Vector3(halfLength, 0.005f, 0f);
        sliderStart.GetComponent<MeshRenderer>().material = sliderIdleMaterial;

        sliderCurrent = transform.GetChild(2).gameObject;
        sliderCurrent.transform.localPosition = new Vector3(0f, 0.005f, 0f);
        sliderCurrent.GetComponent<MeshRenderer>().material = sliderIdleMaterial;

        sliderEnd = transform.GetChild(3).gameObject;
        sliderEnd.transform.localPosition = new Vector3(-halfLength, 0.005f, 0f);
        sliderEnd.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
    }

    private void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger") {
            Debug.Log("enter");
        }
    }

    private void OnTriggerStay(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger") {
            Debug.Log("stay");
        }
    }

    private void OnTriggerExit(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger") {
            Debug.Log("exit");
        }
    }
}
