using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditButtonScript : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject toy;
    private ToyScript toyScript;

    void Start() {
        toyScript = toy.GetComponent<ToyScript>();
    }

    void Update() { }

    void OnMouseDown() {
        if (toyScript.AnimationRecorded()) {
            if (GetComponent<Renderer>().material.color == Color.red) {  // Bad implementation, should either pull from resources or access array of materials
                GetComponent<Renderer>().material.color = Color.green;
                //toyScript.DebugInstantiateSamples();
                Debug.Log("StartEdit");

            } else {
                GetComponent<Renderer>().material.color = Color.red;
                //toyScript.DebugDestroySamples();
                Debug.Log("StopEdit");
            }
        } else {
            Debug.Log("No animation has been recorded");
        }
    }

}
