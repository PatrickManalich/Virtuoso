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
    private float editSliderHalfLength;
    public float bufferDistance;
    private int beginCurrSampleIndex;
    private int endCurrSampleIndex;

    void Start () {
        toyScript = toy.GetComponent<ToyScript>();
        sliderRailHalfLength = sliderRail.transform.localScale.y;
        editSliderHalfLength = editBeginSlider.transform.localScale.x;
        editBeginAid.SetActive(true);
        editBeginAid.transform.position = toyScript.GetPositionAtSliderPercent(0, true);
        editBeginAid.transform.rotation = toyScript.GetRotationAtSliderPercent(0, true);
        beginCurrSampleIndex = toyScript.GetSampleIndexAtSliderPercent(0, true);
        editEndAid.SetActive(true);
        editEndAid.transform.position = toyScript.GetPositionAtSliderPercent(1, false);
        editEndAid.transform.rotation = toyScript.GetRotationAtSliderPercent(1, false);
        endCurrSampleIndex = toyScript.GetSampleIndexAtSliderPercent(1, false);
    }
    
    void Update () { }

    public void SliderGrabbed(bool isBeginSlider, float testPositionX) {
        if (isBeginSlider) {
            if (testPositionX > -sliderRailHalfLength && testPositionX < sliderRailHalfLength) {
                float testBufferedPosition = (testPositionX + (2.0f * editSliderHalfLength) + bufferDistance);
                float sliderPercent = (editBeginSlider.transform.position.x + sliderRailHalfLength) / (sliderRailHalfLength * 2);
                if (testBufferedPosition < editEndSlider.transform.position.x) {
                    editBeginSlider.transform.position = new Vector3(testPositionX, editBeginSlider.transform.position.y, editBeginSlider.transform.position.z);
                    editBeginAid.transform.position = toyScript.GetPositionAtSliderPercent(sliderPercent, true);
                    editBeginAid.transform.rotation = toyScript.GetRotationAtSliderPercent(sliderPercent, true);
                    beginCurrSampleIndex = toyScript.GetSampleIndexAtSliderPercent(sliderPercent, true);
                }
            }
        } else {
            if (testPositionX > -sliderRailHalfLength && testPositionX < sliderRailHalfLength) {
                float testBufferedPosition = (testPositionX - (2.0f * editSliderHalfLength) - bufferDistance);
                float sliderPercent = (editEndSlider.transform.position.x + sliderRailHalfLength) / (sliderRailHalfLength * 2);
                if (testBufferedPosition > editBeginSlider.transform.position.x) {
                    editEndSlider.transform.position = new Vector3(testPositionX, editEndSlider.transform.position.y, editEndSlider.transform.position.z);
                    editEndAid.transform.position = toyScript.GetPositionAtSliderPercent(sliderPercent, false);
                    editEndAid.transform.rotation = toyScript.GetRotationAtSliderPercent(sliderPercent, false);
                    endCurrSampleIndex = toyScript.GetSampleIndexAtSliderPercent(sliderPercent, false);
                }
            }
        }
    }

    public void SliderReleased(bool isBeginSlider) {
        if (isBeginSlider) {
            float snappedSliderPercent = toyScript.GetSnappedSliderPercentAtSampleIndex(beginCurrSampleIndex);
            float snappedSliderPositionX = (snappedSliderPercent * sliderRailHalfLength * 2) - sliderRailHalfLength;
            editBeginSlider.transform.position = new Vector3(snappedSliderPositionX, editBeginSlider.transform.position.y, editBeginSlider.transform.position.z);
        } else {
            float snappedSliderPercent = toyScript.GetSnappedSliderPercentAtSampleIndex(endCurrSampleIndex);
            float snappedSliderPositionX = (snappedSliderPercent * sliderRailHalfLength * 2) - sliderRailHalfLength;
            editEndSlider.transform.position = new Vector3(snappedSliderPositionX, editEndSlider.transform.position.y, editEndSlider.transform.position.z);
        }
    }


}
