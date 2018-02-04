using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditGhostButtonScript : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject toy;
    private ToyScript toyScript;
    public GameObject editGhostButtonText;

    void Start() {
        toyScript = toy.GetComponent<ToyScript>();
    }

    void Update() { }

    void OnMouseDown() {
        if (toyScript.AnimationRecorded()) {
            if (GetComponent<Renderer>().material.color == Color.red)  // Bad implementation, should either pull from resources or access array of materials
                changeState(true);
            else
                changeState(false);
        } else
            Debug.Log("No animation has been recorded");
    }

    public void changeState(bool on) {
        if (on) {
            GetComponent<Renderer>().material.color = Color.green;
            editGhostButtonText.GetComponent<TextMesh>().text = "Ghost\n  On";
            //toyScript.DebugInstantiateSamples();
            Debug.Log("StartEdit");
        } else {
            GetComponent<Renderer>().material.color = Color.red;
            editGhostButtonText.GetComponent<TextMesh>().text = "Ghost\n  Off";
            //toyScript.DebugDestroySamples();
            Debug.Log("StopEdit");
        }
    }

}