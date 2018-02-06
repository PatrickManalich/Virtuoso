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
    private int beginSampleIndex;
    private int endSampleIndex;

    void Start () {
        toyScript = toy.GetComponent<ToyScript>();
        sliderRailHalfLength = sliderRail.transform.localScale.y;
        editSliderHalfLength = editBeginSlider.transform.localScale.x;
        editBeginAid.transform.position = toyScript.GetSamplePos(0);
        editBeginAid.transform.rotation = toyScript.GetSampleRot(0);
        beginSampleIndex = 0;
        editEndAid.transform.position = toyScript.GetSamplePos(toyScript.GetSampleCount() - 1);
        editEndAid.transform.rotation = toyScript.GetSampleRot(toyScript.GetSampleCount() - 1);
        endSampleIndex = toyScript.GetSampleCount() - 1;
    }
    
    void Update () { }

    public void SliderGrabbed(bool isBeginSlider, float testPositionX) {
        if (isBeginSlider) {
            if (testPositionX > -sliderRailHalfLength && testPositionX < sliderRailHalfLength) {
                float testBufferedPosition = (testPositionX + (2.0f * editSliderHalfLength) + bufferDistance);
                float sliderPercent = (editBeginSlider.transform.position.x + sliderRailHalfLength) / (sliderRailHalfLength * 2);
                if (testBufferedPosition < editEndSlider.transform.position.x) {
                    editBeginSlider.transform.position = new Vector3(testPositionX, editBeginSlider.transform.position.y, editBeginSlider.transform.position.z);
                    int sampleIndex = (int)Mathf.Floor(sliderPercent * (toyScript.GetSampleCount() - 1));
                    editBeginAid.transform.position = toyScript.GetSamplePos(sampleIndex);
                    editBeginAid.transform.rotation = toyScript.GetSampleRot(sampleIndex);
                    beginSampleIndex = sampleIndex;
                }
            }
        } else {
            if (testPositionX > -sliderRailHalfLength && testPositionX < sliderRailHalfLength) {
                float testBufferedPosition = (testPositionX - (2.0f * editSliderHalfLength) - bufferDistance);
                float sliderPercent = (editEndSlider.transform.position.x + sliderRailHalfLength) / (sliderRailHalfLength * 2);
                if (testBufferedPosition > editBeginSlider.transform.position.x) {
                    editEndSlider.transform.position = new Vector3(testPositionX, editEndSlider.transform.position.y, editEndSlider.transform.position.z);
                    int sampleIndex = (int)Mathf.Ceil(sliderPercent * (toyScript.GetSampleCount() - 1));
                    editEndAid.transform.position = toyScript.GetSamplePos(sampleIndex);
                    editEndAid.transform.rotation = toyScript.GetSampleRot(sampleIndex);
                    endSampleIndex = sampleIndex;
                }
            }
        }
    }

    public void SliderReleased(bool isBeginSlider) {
        if (isBeginSlider) {
            float snappedSliderPercent = (float) beginSampleIndex/ ((float) toyScript.GetSampleCount() - 1.0f);
            float snappedSliderPositionX = (snappedSliderPercent * sliderRailHalfLength * 2) - sliderRailHalfLength;
            editBeginSlider.transform.position = new Vector3(snappedSliderPositionX, editBeginSlider.transform.position.y, editBeginSlider.transform.position.z);
        } else {
            float snappedSliderPercent = (float) endSampleIndex / ((float)toyScript.GetSampleCount() - 1.0f);
            float snappedSliderPositionX = (snappedSliderPercent * sliderRailHalfLength * 2) - sliderRailHalfLength;
            editEndSlider.transform.position = new Vector3(snappedSliderPositionX, editEndSlider.transform.position.y, editEndSlider.transform.position.z);
        }
    }

    public int GetBeginSampleIndex() {
        return beginSampleIndex;
    }

    public int GetEndSampleIndex() {
        return endSampleIndex;
    }

}
