using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSliderScript : MonoBehaviour {

    public enum SliderType { Begin, End };
    public SliderType sliderType;
    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject toy;
    private ToyScript toyScript;
    public GameObject sliderRail;
    public GameObject editBeginAid;
    public GameObject editEndAid;
    public GameObject otherEditSlider;
    private float sliderRailHalfLength;
    private float editSliderHalfLength;
    public float bufferDistance;



    void Start() {
        toyScript = toy.GetComponent<ToyScript>();
        if (toyScript.AnimationRecorded())
            AdjustAidPosition();
        sliderRailHalfLength = sliderRail.transform.localScale.y;
        editSliderHalfLength = transform.localScale.x;
    }

    void Update() { }


    private void AdjustAidPosition() {
        float sliderPercent = (transform.position.x + sliderRailHalfLength) / (sliderRailHalfLength * 2);
        if (sliderType ==  SliderType.Begin) {
            editBeginAid.transform.position = toyScript.GetPositionAtSliderPercent(sliderPercent);
            editBeginAid.transform.rotation = toyScript.GetRotationAtSliderPercent(sliderPercent);
        } else {
            editEndAid.transform.position = toyScript.GetPositionAtSliderPercent(sliderPercent);
            editEndAid.transform.rotation = toyScript.GetRotationAtSliderPercent(sliderPercent);
        }
    }
    void OnMouseDown() {
        if (toyScript.AnimationRecorded()) {
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        } else {
            Debug.Log("No animation has been recorded");
        }
    }

    void OnMouseDrag() {
        if (toyScript.AnimationRecorded()) {
            Vector3 currScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 currPosition = Camera.main.ScreenToWorldPoint(currScreenPoint) + offset;
            if (currPosition.x > -sliderRailHalfLength && currPosition.x < sliderRailHalfLength) {  // CHANGE SO THEY DON'T CRASH
                if (sliderType == SliderType.Begin) {
                    if (otherEditSlider.transform.position.x > (currPosition.x + (2 * editSliderHalfLength) + bufferDistance)) {
                        transform.position = new Vector3(currPosition.x, transform.position.y, transform.position.z);
                        AdjustAidPosition();
                    }

                } else {
                    if (otherEditSlider.transform.position.x < (currPosition.x - (2 * editSliderHalfLength) - bufferDistance)) {
                        transform.position = new Vector3(currPosition.x, transform.position.y, transform.position.z);
                        AdjustAidPosition();
                    }
                }
            }
        }
    }


}