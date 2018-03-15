using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandContainerScript : MonoBehaviour {

    private Transform compoundColliderTransform;
    private GameObject closestBand;
    private bool bufferFinished;

    public float bufferLength;

    private void Awake() {
        compoundColliderTransform = transform.GetChild(0);
        compoundColliderTransform.localPosition = new Vector3(-0.08f, 0.005f, -0.205f);
        compoundColliderTransform.Rotate(0f, 0f, -13.6f);
        compoundColliderTransform.localScale = new Vector3(0.09f, 0.07f, 0.18f);
        closestBand = null;
        bufferFinished = true;
    }

    private void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger") {
            closestBand = GetClosestBand(otherCollider.transform.position);
            closestBand.GetComponent<BandScript>().Hovering();
            bufferFinished = false;
            StartCoroutine(Wait());
        }
    }

    private void OnTriggerStay(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger" && bufferFinished) {
            GameObject newClosestBand = GetClosestBand(otherCollider.transform.position);
            if(newClosestBand.name != closestBand.name) {
                closestBand.GetComponent<BandScript>().Unhovering();
                closestBand = newClosestBand;
                closestBand.GetComponent<BandScript>().Hovering();
                bufferFinished = false;
                StartCoroutine(Wait());
            } else if (Mathf.Abs(Vector3.Distance(closestBand.transform.position, otherCollider.transform.position)) < 0.025f) {
                StartCoroutine(closestBand.GetComponent<BandScript>().Toggle());
                bufferFinished = false;
                StartCoroutine(Wait());
            }
        }
    }

    private void OnTriggerExit(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger") {
            closestBand.GetComponent<BandScript>().Unhovering();
            bufferFinished = true;
        }
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(bufferLength);
        bufferFinished = true;
        yield return null;
    }

    private GameObject GetClosestBand(Vector3 fingerTriggerPosition) {
        GameObject closestBand = null;
        float minDistance = 0f;
        bool firstBand = true;
        foreach (Transform child in transform) {
            if (child.gameObject.activeSelf && child.tag == "Band") {
                float testDistance = Vector3.Distance(child.transform.position, fingerTriggerPosition);
                if (firstBand) {
                    closestBand = child.gameObject;
                    minDistance = testDistance;
                    firstBand = false;
                } else if ( testDistance < minDistance) {
                    closestBand = child.gameObject;
                    minDistance = testDistance; 
                }
            }
        }
        return closestBand;
    }
}
