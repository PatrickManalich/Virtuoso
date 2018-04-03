using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderFieldScript : MonoBehaviour {

    private DummyScript dummyScript;                // The dummy script of the dummy Game Object
    private int dummySampleCount;                   // The sample count of the dummy, cached for efficiency
    private const float localHalfLength = 0.1f;         // Half the length of the slider field in local scale
    private const float worldHalfLength = 0.1f * 0.82f; // Half the length of the slider field in world scale
    private const float indicatorOffset = -0.015f;      // The offset required to place the indicator above the sliders
    private Vector3 localCenter;                    // The local center of the axis the sliders follow
    private Transform compoundColliderTransform;    // The transform of compound collider, used for extra collider control
    private GameObject startSlider;                 // The start slider, which should be a child of the slider field
    private GameObject currentSlider;               // The current slider, which should be a child of the slider field
    private GameObject endSlider;                   // The end slider, which should be a child of the slider field
    private int startSliderSampleIndex;             // The sample index the start slider is currently over
    private int endSliderSampleIndex;               // The sample index the end slider is currently over
    private GameObject sliderIndicator;             // The slider indicator, which should be a child of the slider field
    private GameObject closestSlider;               // The slider closest to the finger trigger when within the compound collider

    public GameObject dummy;                        // The dummy Game Object used for getting its script
    public GameObject startAid;                     // The start aid Game Object
    public GameObject endAid;                       // The end aid Game Object
    public Material sliderIdleMaterial;             // The material for all sliders while idle
    public Material sliderSlidingMaterial;          // The material for all sliders while sliding
    public float slideDistance;                     // The minimum distance from finger toggle to slider that is required to slide it

    private void Awake() {
        dummyScript = dummy.GetComponent<DummyScript>();
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
            // Initialize start slider
        startSlider = transform.GetChild(1).gameObject;
        startSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
            // Initialize current slider
        currentSlider = transform.GetChild(2).gameObject;
        currentSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
            // Initialize end slider
        endSlider = transform.GetChild(3).gameObject;
        endSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
            // Initialize slider indicator
        sliderIndicator = transform.GetChild(4).gameObject;
        sliderIndicator.SetActive(false);
    }

    private void Start() {
        dummySampleCount = dummyScript.GetSampleCount();
        // Initialize aids
        startAid.transform.position = dummy.transform.position;
        startAid.transform.localScale = dummy.transform.localScale * 0.99f; // Prevents mesh clashing when overlaid
        endAid.transform.position = dummy.transform.position;
        endAid.transform.localScale = dummy.transform.localScale * 0.99f;   // Prevents mesh clashing when overlaid
    }

    private void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger") {
            closestSlider = GetClosestSlider(otherCollider.transform.position);
            sliderIndicator.SetActive(true);
            sliderIndicator.transform.localPosition = V3E.SetZ(closestSlider.transform.localPosition, indicatorOffset);
        }
    }

    private void OnTriggerStay(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger") {
            GameObject newClosestSlider = GetClosestSlider(otherCollider.transform.position);
            if (newClosestSlider.name != closestSlider.name) {
                    // If hovering over a different slider, switch which slider is being hovered over
                if (closestSlider.GetComponent<MeshRenderer>().sharedMaterial.Equals(sliderSlidingMaterial))
                    closestSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;

                closestSlider = newClosestSlider;
                sliderIndicator.transform.localPosition = V3E.SetZ(closestSlider.transform.localPosition, indicatorOffset);
            } else if (Mathf.Abs(Vector3.Distance(closestSlider.transform.position, otherCollider.transform.position)) < slideDistance) {
                    // If closest slider is within slide distance, change material and slide the closest slider
                if (closestSlider.GetComponent<MeshRenderer>().sharedMaterial.Equals(sliderIdleMaterial))
                    closestSlider.GetComponent<MeshRenderer>().material = sliderSlidingMaterial;

                SlideClosestSlider(otherCollider.transform.position);
            } else {
                    // If closest slider is out of slide distance, change material back to idle if it was changed
                if (closestSlider.GetComponent<MeshRenderer>().sharedMaterial.Equals(sliderSlidingMaterial))
                    closestSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider otherCollider) {
        if (otherCollider.tag == "FingerTrigger" && closestSlider) {
                // Automatically unhover from closest slider
            closestSlider.GetComponent<MeshRenderer>().material = sliderIdleMaterial;
            closestSlider = null;
            sliderIndicator.SetActive(false);
        }
    }

    /* Takes in the finger trigger position and returns the closest slider to it. */
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

        // If the startSlider or endSlider and the currentSlider are on top of each other, return the current slider so it will
        // always be between the two
        if (closestSlider == startSlider && Vector3.Distance(startSlider.transform.position, currentSlider.transform.position) == 0) 
            closestSlider = currentSlider;
        else if (closestSlider == endSlider && Vector3.Distance(endSlider.transform.position, currentSlider.transform.position) == 0)
            closestSlider = currentSlider;

        return closestSlider;
    }

    /* Takes in a finger trigger position and sliders the closest slider in one dimension relative to the finger
     * slider position (if the new slider position will be on the slider field). If the slider is the start slider
     * or the end slider then they will snap to the floored or ceiled sample respectively. Else if the slider is
     * the current slider, it will adjust the position with a smooth transition using t. */
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

        float testDistance = Vector3.Distance(pointInOneDimension, worldCenter);

        if (testDistance < worldHalfLength) {
            
            if (Vector3.Distance(pointInOneDimension, transform.TransformPoint(V3E.SetX(localCenter, -localHalfLength))) <
                Vector3.Distance(pointInOneDimension, transform.TransformPoint(V3E.SetX(localCenter, localHalfLength))))
                testDistance *= -1;   // Closer to left side, so make negative since Vector3.Distance() is only magnitude

            float sliderPercent = (testDistance - worldHalfLength) / (worldHalfLength * -2);

            if (closestSlider == startSlider) {
                    // Find the floored sample index, set the start slider sample index to this, then find and set new x position,
                    // and adjust slider indicator and start aid
                int flooredSampleIndex = (int)Mathf.Floor(sliderPercent * (dummySampleCount - 1));
                startSliderSampleIndex = flooredSampleIndex;
                float flooredSamplePercent = (float)startSliderSampleIndex / ((float)dummyScript.GetSampleCount() - 1.0f);
                float newX = (flooredSamplePercent * localHalfLength * -2) + localHalfLength;
                startSlider.transform.localPosition = V3E.SetX(localCenter, newX);
                sliderIndicator.transform.localPosition = V3E.SetZ(closestSlider.transform.localPosition, indicatorOffset);
                startAid.transform.position = dummyScript.GetSamplePosition(startSliderSampleIndex);
                startAid.transform.rotation = dummyScript.GetSampleRotation(startSliderSampleIndex);
            } else if(closestSlider == currentSlider) {
                    // Find the floored sample index, find the t, adjust the current slider, then adjust slider indicator and dummy
                int flooredSampleIndex = (int)Mathf.Floor(sliderPercent * (dummySampleCount - 1));
                float flooredSamplePercent = (float)flooredSampleIndex / ((float)dummySampleCount - 1.0f);
                float t = (sliderPercent - flooredSamplePercent) / (1.0f / ((float)dummySampleCount - 1.0f));
                AdjustSlider("CurrentSlider", flooredSampleIndex, t);
                sliderIndicator.transform.localPosition = V3E.SetZ(closestSlider.transform.localPosition, indicatorOffset);
                dummyScript.Adjust(flooredSampleIndex, t);
            } else {    // end slider
                    // Find the ceiled sample index, set the end slider sample index to this, then find and set new x position,
                    // and adjust slider indicator and end aid
                int ceiledSampleIndex = (int)Mathf.Ceil(sliderPercent * (dummySampleCount - 1));
                endSliderSampleIndex = ceiledSampleIndex;
                float ceiledSamplePercent = (float)endSliderSampleIndex / ((float)dummyScript.GetSampleCount() - 1.0f);
                float newX = (ceiledSamplePercent * localHalfLength * -2) + localHalfLength;
                endSlider.transform.localPosition = V3E.SetX(localCenter, newX);
                sliderIndicator.transform.localPosition = V3E.SetZ(closestSlider.transform.localPosition, indicatorOffset);
                endAid.transform.position = dummyScript.GetSamplePosition(endSliderSampleIndex);
                endAid.transform.rotation = dummyScript.GetSampleRotation(endSliderSampleIndex);
            }
        }
    }

    /* Takes in a slider name, a sample index, and a t. Based on what the slider name is, will use the values to adjust the position,
     * indicator, and aids (unless current slider). Both start slider and end slider don't use t parameter. */
    public void AdjustSlider(string sliderName, int sampleIndex, float t) {
        if(sliderName == "StartSlider") {
            startSliderSampleIndex = sampleIndex;
            float samplePercent = (float)startSliderSampleIndex / ((float)dummySampleCount - 1.0f);
            float newX = (samplePercent * localHalfLength * -2) + localHalfLength;
            startSlider.transform.localPosition = V3E.SetX(localCenter, newX);
            if (sliderIndicator.activeSelf)
                sliderIndicator.transform.localPosition = V3E.SetZ(startSlider.transform.localPosition, indicatorOffset);
            startAid.transform.position = dummyScript.GetSamplePosition(startSliderSampleIndex);
            startAid.transform.rotation = dummyScript.GetSampleRotation(startSliderSampleIndex);
        } else if(sliderName == "CurrentSlider") {
            float samplePercent = (float)sampleIndex / ((float)dummySampleCount - 1.0f);
            float sliderPercent = t * (1.0f / ((float)dummySampleCount - 1.0f)) + samplePercent;
            float newX = (sliderPercent * localHalfLength * -2) + localHalfLength;
            currentSlider.transform.localPosition = V3E.SetX(localCenter, newX);
            if (sliderIndicator.activeSelf)
                sliderIndicator.transform.localPosition = V3E.SetZ(currentSlider.transform.localPosition, indicatorOffset);
        } else if(sliderName == "EndSlider") {
            endSliderSampleIndex = sampleIndex;
            float samplePercent = (float)endSliderSampleIndex / ((float)dummySampleCount - 1.0f);
            float newX = (samplePercent * localHalfLength * -2) + localHalfLength;
            endSlider.transform.localPosition = V3E.SetX(localCenter, newX);
            if (sliderIndicator.activeSelf)
                sliderIndicator.transform.localPosition = V3E.SetZ(endSlider.transform.localPosition, indicatorOffset);
            endAid.transform.position = dummyScript.GetSamplePosition(endSliderSampleIndex);
            endAid.transform.rotation = dummyScript.GetSampleRotation(endSliderSampleIndex);
        }
    }

    /* Returns the start slider sample index. */
    public int GetStartSliderSampleIndex() { return startSliderSampleIndex; }

    /* Returns the end slider sample index. */
    public int GetEndSliderSampleIndex() { return endSliderSampleIndex; }

}
