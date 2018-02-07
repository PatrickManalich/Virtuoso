using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditGhostButtonScript : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject editGhostButtonText;
    public GameObject toy;
    private ToyScript toyScript;
    public GameObject editSliderController;
    private EditSliderControllerScript editSliderControllerScript;

    void Awake() {
        toyScript = toy.GetComponent<ToyScript>();
        editSliderControllerScript = editSliderController.GetComponent<EditSliderControllerScript>();
    }

    void OnMouseDown() {
        if (GetComponent<Renderer>().material.color == Color.red)  // Bad implementation, should either pull from resources or access array of materials
            ChangeState(true);
        else
            ChangeState(false);
    }

    public void ChangeState(bool on) {
        if (on) {
            GetComponent<Renderer>().material.color = Color.green;
            editGhostButtonText.GetComponent<TextMesh>().text = "Ghost\n  On";
            //toyScript.DebugInstantiateSamples();
            toyScript.StartGhosting(editSliderControllerScript.GetBeginSampleIndex(), editSliderControllerScript.GetEndSampleIndex());
        } else {
            GetComponent<Renderer>().material.color = Color.red;
            editGhostButtonText.GetComponent<TextMesh>().text = "Ghost\n  Off";
            //toyScript.DebugDestroySamples();
            toyScript.StopGhosting();
         }
    }

}