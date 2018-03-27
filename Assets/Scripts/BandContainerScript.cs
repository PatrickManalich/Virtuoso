using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandContainerScript : MonoBehaviour {

    private Transform compoundColliderTransform;    // The transform of compound collider, used for extra collider control
    private GameObject closestBand;                 // The band closest to the finger trigger when within the compound collider
    private bool bufferFinished;                    /* This returns true if buffer has finished. Buffer is used for both waiting on
                                                     * band animations to finish and to prevent multiple band toggles */

    public float bufferSeconds;                     // The number of seconds the buffer lasts
    public float toggleDistance;                    // The minimum distance from finger toggle to band that is required to toggle it

    private void Awake() {
            // Initialize the local position, rotation, and scale of compound collider
        compoundColliderTransform = transform.GetChild(0);
        compoundColliderTransform.localPosition = new Vector3(-0.08f, 0.005f, -0.205f);
        compoundColliderTransform.Rotate(0f, 0f, -13.6f);
        compoundColliderTransform.localScale = new Vector3(0.09f, 0.07f, 0.18f);            
    }

    private void Start() {
            // Initialize closestBand and bufferFinished
        closestBand = null;
        bufferFinished = true;
    }

    private void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger") {
                // Find the closest band, start hovering animation, and begin waiting for buffer seconds
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
                    // If hovering over a different band, switch which band is being hovered over
                closestBand.GetComponent<BandScript>().Unhovering();
                closestBand = newClosestBand;
                closestBand.GetComponent<BandScript>().Hovering();
                bufferFinished = false;
                StartCoroutine(Wait());
            } else if (Mathf.Abs(Vector3.Distance(closestBand.transform.position, otherCollider.transform.position)) < toggleDistance) {
                    // If closest band is within toggle distance, toggle the closest band
                StartCoroutine(closestBand.GetComponent<BandScript>().Toggle());
                bufferFinished = false;
                StartCoroutine(Wait());
            }
        }
    }

    private void OnTriggerExit(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger" && closestBand) {
                // Automatically unhover from closest band
            closestBand.GetComponent<BandScript>().Unhovering();
            closestBand = null;
            bufferFinished = true;
        }
    }

    // Waits for buffer seconds, then sets bufferFinished to true, allowing further action to take place
    private IEnumerator Wait() {
        yield return new WaitForSeconds(bufferSeconds);
        bufferFinished = true;
        yield return null;
    }

    // Takes in the finger trigger position and returns the closest band to it
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
