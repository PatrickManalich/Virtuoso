using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandContainerScript : MonoBehaviour {

    private GameObject closestBand;
    private bool bufferTimeFinished;

    public float bufferTime;

    private void Awake() {
        closestBand = null;
        bufferTimeFinished = true;
    }

    private void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.tag == "BandTrigger") {
            closestBand = GetClosestBand(otherCollider.transform.position);
            closestBand.GetComponent<BandScript>().Hovering();
        }
    }

    private void OnTriggerStay(Collider otherCollider) {
        if (otherCollider.tag == "BandTrigger") {
            GameObject newClosestBand = GetClosestBand(otherCollider.transform.position);
            if(newClosestBand.name == closestBand.name) {
                if (Mathf.Abs(Vector3.Distance(closestBand.transform.position, otherCollider.transform.position)) < 0.025f && bufferTimeFinished) {
                    bufferTimeFinished = false;
                    StartCoroutine(closestBand.GetComponent<BandScript>().Toggle());
                    StartCoroutine(WaitBufferTime());
                }
            } else {
                closestBand.GetComponent<BandScript>().Unhovering();
                closestBand = newClosestBand;
                closestBand.GetComponent<BandScript>().Hovering();
            }
        }
    }

    private void OnTriggerExit(Collider otherCollider) {
        if (otherCollider.tag == "BandTrigger")
            closestBand.GetComponent<BandScript>().Unhovering();
    }

    private IEnumerator WaitBufferTime() {
        yield return new WaitForSeconds(bufferTime);
        bufferTimeFinished = true;
        yield return null;
    }

    private GameObject GetClosestBand(Vector3 bandTriggerPosition) {
        GameObject closestBand = null;
        float minDistance = 0f;
        bool firstBand = true;
        foreach (Transform band in transform) {
            if (band.gameObject.activeSelf) {
                float testDistance = Vector3.Distance(band.transform.position, bandTriggerPosition);
                if (firstBand) {
                    closestBand = band.gameObject;
                    minDistance = testDistance;
                    firstBand = false;
                } else if ( testDistance < minDistance) {
                    closestBand = band.gameObject;
                    minDistance = testDistance; 
                }
            }
        }
        return closestBand;
    }
}
