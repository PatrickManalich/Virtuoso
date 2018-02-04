using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySliderScript : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject toy;
    private ToyScript toyScript;
    public GameObject sliderRail;
    private float sliderRailHalfLength;


    void Start() {
        toyScript = toy.GetComponent<ToyScript>();
        sliderRailHalfLength = sliderRail.transform.localScale.y;
    }

    void Update() { }

    void OnMouseDown() {
        if (toyScript.AnimationRecorded()) {
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        } else
            Debug.Log("No animation has been recorded");
    }

    void OnMouseDrag() {
        if (toyScript.AnimationRecorded()) {
            Vector3 testScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 testPosition = Camera.main.ScreenToWorldPoint(testScreenPoint) + offset;
            float testPositionX = testPosition.x;
            if (testPositionX > -sliderRailHalfLength && testPositionX < sliderRailHalfLength) {
                transform.position = new Vector3(testPositionX, transform.position.y, transform.position.z); ;
                float sliderPercent = (testPositionX + sliderRailHalfLength) / (sliderRailHalfLength * 2);
                toyScript.CalibrateWithPlaySlider(sliderPercent);
            }
        }
    }

    public void CalibrateWithToy(float sliderPercent) {
        if (sliderPercent < 0f)
            sliderPercent = 0f;
        else if (sliderPercent > 1f)
            sliderPercent = 1f;
        float newX = (sliderPercent * (sliderRailHalfLength * 2)) - sliderRailHalfLength;
        Vector3 newPosition = new Vector3(newX, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }

}