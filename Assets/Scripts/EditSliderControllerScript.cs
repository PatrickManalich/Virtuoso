using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSliderControllerScript : MonoBehaviour {

    public GameObject toy;
    private ToyScript toyScript;
    public GameObject sliderRail;
    public GameObject editBeginSlider;
    public GameObject editEndSlider;
    public GameObject editBeginAid;
    public GameObject editEndAid;
    private float sliderRailHalfLength;
    public float bufferPercent;
    private int beginSampleIndex;
    private int endSampleIndex;

    void Awake () {
        toyScript = toy.GetComponent<ToyScript>();
        sliderRailHalfLength = sliderRail.transform.localScale.y;
    }

    public void SliderGrabbed(bool isBeginSlider, float testPositionX) {
        if (testPositionX > -sliderRailHalfLength && testPositionX < sliderRailHalfLength) {
            if (isBeginSlider) {
                float testBeginSliderPercent = (testPositionX + sliderRailHalfLength) / (sliderRailHalfLength * 2);
                float currEndSliderPercent = (editEndSlider.transform.position.x + sliderRailHalfLength) / (sliderRailHalfLength * 2);
                if ((currEndSliderPercent - testBeginSliderPercent) > bufferPercent) {
                    editBeginSlider.transform.position = new Vector3(testPositionX, editBeginSlider.transform.position.y, editBeginSlider.transform.position.z);
                    beginSampleIndex = (int)Mathf.Floor(testBeginSliderPercent * (toyScript.GetSampleCount() - 1));
                    editBeginAid.transform.position = toyScript.GetSamplePos(beginSampleIndex);
                    editBeginAid.transform.rotation = toyScript.GetSampleRot(beginSampleIndex);
                }
            } else {
                float currBeginSliderPercent = (editBeginSlider.transform.position.x + sliderRailHalfLength) / (sliderRailHalfLength * 2);
                float testEndSliderPercent = (testPositionX + sliderRailHalfLength) / (sliderRailHalfLength * 2);
                if ((testEndSliderPercent - currBeginSliderPercent) > bufferPercent) {
                    editEndSlider.transform.position = new Vector3(testPositionX, editEndSlider.transform.position.y, editEndSlider.transform.position.z);
                    endSampleIndex = (int)Mathf.Ceil(testEndSliderPercent * (toyScript.GetSampleCount() - 1));
                    editEndAid.transform.position = toyScript.GetSamplePos(endSampleIndex);
                    editEndAid.transform.rotation = toyScript.GetSampleRot(endSampleIndex);
                }
            }
        }            
    }

    public void SliderReleased(bool isBeginSlider) {
        if (isBeginSlider) {
            float snappedSliderPercent = (float) beginSampleIndex/ ((float) toyScript.GetSampleCount() - 1.0f);
            float snappedSliderPositionX = (snappedSliderPercent * sliderRailHalfLength * 2) - sliderRailHalfLength;
            editBeginSlider.transform.position = new Vector3(snappedSliderPositionX, editBeginSlider.transform.position.y, editBeginSlider.transform.position.z);
            toyScript.ChangeLocation(beginSampleIndex, 0f);
        } else {
            float snappedSliderPercent = (float) endSampleIndex / ((float)toyScript.GetSampleCount() - 1.0f);
            float snappedSliderPositionX = (snappedSliderPercent * sliderRailHalfLength * 2) - sliderRailHalfLength;
            editEndSlider.transform.position = new Vector3(snappedSliderPositionX, editEndSlider.transform.position.y, editEndSlider.transform.position.z);
        }
    }

    public int GetBeginSampleIndex() { return beginSampleIndex; }

    public int GetEndSampleIndex() { return endSampleIndex; }

    public void SetupLocations() {
        float testBeginSliderPercent = (float)toyScript.GetLastSampleIndex() / ((float)toyScript.GetSampleCount() - 1.0f);
        float testEndSliderPercent = testBeginSliderPercent + bufferPercent;
        if (testEndSliderPercent < 1) { // EditEndSlider won't go out of slider rail range
            beginSampleIndex = toyScript.GetLastSampleIndex();
            float newBeginX = (testBeginSliderPercent * (sliderRailHalfLength * 2)) - sliderRailHalfLength;
            editBeginSlider.transform.position = new Vector3(newBeginX, editBeginSlider.transform.position.y, editBeginSlider.transform.position.z);
            endSampleIndex = (int)Mathf.Ceil(testEndSliderPercent * (toyScript.GetSampleCount() - 1));
            float newSnappedSliderPercent = (float)endSampleIndex / ((float)toyScript.GetSampleCount() - 1.0f);
            float newSnappedSliderPositionX = (newSnappedSliderPercent * sliderRailHalfLength * 2) - sliderRailHalfLength;
            editEndSlider.transform.position = new Vector3(newSnappedSliderPositionX, editEndSlider.transform.position.y, editEndSlider.transform.position.z);
            toyScript.ChangeLocation(beginSampleIndex, 0f);
        } else {  // EditEndSlider will go out of range
            Debug.Log("Please leave more room for editing, setting default locations");
            beginSampleIndex = 0;
            editBeginSlider.transform.position = new Vector3(-sliderRailHalfLength, editBeginSlider.transform.position.y, editBeginSlider.transform.position.z);
            endSampleIndex = toyScript.GetSampleCount() - 1;
            editEndSlider.transform.position = new Vector3(sliderRailHalfLength, editEndSlider.transform.position.y, editEndSlider.transform.position.z);
            toyScript.ChangeLocation(beginSampleIndex, 0f);
        }
        editBeginAid.transform.position = toyScript.GetSamplePos(beginSampleIndex);
        editBeginAid.transform.rotation = toyScript.GetSampleRot(beginSampleIndex);
        editEndAid.transform.position = toyScript.GetSamplePos(endSampleIndex);
        editEndAid.transform.rotation = toyScript.GetSampleRot(endSampleIndex);
    }

}
