using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderFieldScript : MonoBehaviour {


    private const float localHalfLength = 0.1f;          // Half the length of the slider field in local scale
    private const float worldHalfLength = 0.1f * 0.82f;  // Half the length of the slider field in world scale
    private Vector3 localCenter;                    // The local center of the axis the sliders follow
    private Transform compoundColliderTransform;    // The transform of compound collider, used for extra collider control

    private GameObject startSlider;                 // The start slider, which should be a child of the slider field
    private GameObject currentSlider;               // The current slider, which should be a child of the slider field
    private GameObject endSlider;                   // The end slider, which should be a child of the slider field
    private GameObject closestSlider;               // The slider closest to the finger trigger when within the compound collider
    private bool wasSliding;                        // True if the closest slider was sliding, used for giving the press effect

    public Material sliderIdleMaterial;             // The material for all sliders while idle
    public Material sliderHoveringMaterial;         // The material for all sliders while hovering
    public float slideDistance;                     // The minimum distance from finger toggle to slider that is required to slide it

    private void Awake() {
            // Initialize the local position, rotation, and scale of the slider field
        transform.localPosition = new Vector3(-0.086f, -0.055f, -0.2047f);
        transform.Rotate(-75, 90, 0);
        transform.localScale = new Vector3(0.82f, 0.82f, 0.82f);

            // Initialize the local position, rotation, and scale of compound collider
        compoundColliderTransform = transform.GetChild(0);
        compoundColliderTransform.localPosition = new Vector3(0.0f, 0.03f, -0.005f);
        compoundColliderTransform.localScale = new Vector3(0.215f, 0.09f, 0.06f);

            //Initialize the local center
        localCenter = new Vector3(0f, 0.005f, 0f);

            // Initialize slider start
        startSlider = transform.GetChild(1).gameObject;
        startSlider.transform.localPosition = V3E.SetX(localCenter, localHalfLength);
        startSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
            // Initialize slider current
        currentSlider = transform.GetChild(2).gameObject;
        currentSlider.transform.localPosition = localCenter;
        currentSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
            // Initialize slider end
        endSlider = transform.GetChild(3).gameObject;
        endSlider.transform.localPosition = V3E.SetX(localCenter, -localHalfLength);
        endSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
    }

    private void Start() {
        closestSlider = null;
        wasSliding = false;
    }

    private void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger") {
            closestSlider = GetClosestSlider(otherCollider.transform.position);
            closestSlider.GetComponent<MeshRenderer>().material = sliderHoveringMaterial;
        }
    }

    private void OnTriggerStay(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger") {
            GameObject newClosestSlider = GetClosestSlider(otherCollider.transform.position);
            if (newClosestSlider.name != closestSlider.name) {
                    // If hovering over a different slider, switch which slider is being hovered over
                closestSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
                if(wasSliding) {
                    wasSliding = false;
                }
                closestSlider = newClosestSlider;
                closestSlider.GetComponent<MeshRenderer>().material = sliderHoveringMaterial;
            } else if (Mathf.Abs(Vector3.Distance(closestSlider.transform.position, otherCollider.transform.position)) < slideDistance) {
                    // If closest slider is within slide distance, slide the closest slider
                SlideClosestSlider(otherCollider.transform.position);
                wasSliding = true;
            }
        }
    }

    private void OnTriggerExit(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger" && closestSlider) {
                // Automatically unhover from closest slider
            closestSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
            if (wasSliding)
                wasSliding = false;
            closestSlider = null;
        }
    }

    // Takes in the finger trigger position and returns the closest slider to it
    private GameObject GetClosestSlider(Vector3 fingerTriggerPosition) {
        GameObject closestSlider = null;
        float minDistance = 0f;
        bool firstSlider = true;
        foreach (Transform child in transform) {
            if (child.gameObject.activeSelf && child.tag == "Slider") {
                float testDistance = Vector3.Distance(child.transform.position, fingerTriggerPosition);
                if (firstSlider) {
                    closestSlider = child.gameObject;
                    minDistance = testDistance;
                    firstSlider = false;
                } else if (testDistance < minDistance) {
                    closestSlider = child.gameObject;
                    minDistance = testDistance;
                }
            }
        }
        return closestSlider;
    }

    // 
    private void SlideClosestSlider(Vector3 fingerTriggerPosition) {
        Vector3 worldCenter = transform.TransformPoint(localCenter);
            // Project point into two dimensions on slider field
        Vector3 v = fingerTriggerPosition - worldCenter;
        Vector3 d = Vector3.Project(v, transform.up);
        Vector3 pointInTwoDimensions = fingerTriggerPosition - d;
            // Project point into one dimension on slider field
        Vector3 v2 = pointInTwoDimensions - worldCenter;
        Vector3 d2 = Vector3.Project(v2, transform.forward);
        Vector3 pointInOneDimension = pointInTwoDimensions - d2;

        if (Vector3.Distance(pointInOneDimension, worldCenter) < worldHalfLength)
            closestSlider.transform.position = pointInOneDimension;

        //if (testPositionX > -halfLength && testPositionX < halfLength) { }
        //    if (closestSlider == startSlider) {
        //        Debug.Log("here");
        //        closestSlider.transform.localPosition = V3E.SetX(closestSlider.transform.localPosition, testPositionX);
        //    } else if (closestSlider == currentSlider) {
        //    } else if (closestSlider == endSlider) {
        //    } else
        //        Debug.Log("closest slider is null");
        //}
    }

    // Takes in a start point, an end point, and a duration and creates a line from start to end for
    // a duration amount of seconds. Used for debugging 3D vector issues.
    private void DebugDrawLine(Vector3 start, Vector3 end, float duration) {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.startWidth = 0.005f;
        lr.endWidth = 0.001f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

}
