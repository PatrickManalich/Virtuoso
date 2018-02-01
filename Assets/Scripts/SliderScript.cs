using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderScript : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private GameObject toy;
    private ToyScript toyScript;
    private GameObject sliderRail;

    void Start() {
        toy = GameObject.Find("Toy");
        toyScript = (ToyScript)toy.GetComponent(typeof(ToyScript));
        sliderRail = GameObject.Find("SliderRail");
    }

    void Update() { }

    void OnMouseDown() {
        if (toyScript.AnimationRecorded()) {
            toyScript.Reset();
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
            Vector3 currPositionRestricted = new Vector3(currPosition.x, transform.position.y, transform.position.z);
            float sliderRailHalfLength = sliderRail.transform.localScale.y;
            if (currPosition.x > -sliderRailHalfLength && currPosition.x < sliderRailHalfLength) {
                transform.position = currPositionRestricted;
                float sliderPercent = (currPosition.x + sliderRailHalfLength) / (sliderRailHalfLength * 2);
                toyScript.MatchSliderPosition(sliderPercent);
            }
        }
    }

}