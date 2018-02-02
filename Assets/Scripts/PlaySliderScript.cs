using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySliderScript : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject toy;
    private ToyScript toyScript;
    public GameObject sliderRail;

    void Start() {
        toyScript = toy.GetComponent<ToyScript>();
    }

    void Update() { }

    void OnMouseDown() {
        if (toyScript.AnimationRecorded()) {
            //toyScript.Reset();
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

    public void MatchToyPosition(float sliderPercent) {
        float sliderRailHalfLength = sliderRail.transform.localScale.y;
        float newX = (sliderPercent * (sliderRailHalfLength * 2)) - sliderRailHalfLength;
        Vector3 newPosition = new Vector3(newX, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }

}