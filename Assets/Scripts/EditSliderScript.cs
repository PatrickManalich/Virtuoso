using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSliderScript : MonoBehaviour {

    public bool isBeginSlider;
    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject editSliderController;
    private EditSliderControllerScript editSliderControllerScript;
    
    void Start() {
        editSliderControllerScript = editSliderController.GetComponent<EditSliderControllerScript>();
    }

    void Update() { }

    void OnMouseDown() {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag() {
        Vector3 testScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 testPosition = Camera.main.ScreenToWorldPoint(testScreenPoint) + offset;
        float testPositionX = testPosition.x;
        editSliderControllerScript.SliderGrabbed(isBeginSlider, testPositionX);
    }
     void OnMouseUp() {
        editSliderControllerScript.SliderReleased(isBeginSlider);
    }

}