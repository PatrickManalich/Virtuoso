using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {

    private GameObject toy;
    private ToyScript toyScript;

    void Start() {
        toy = GameObject.Find("Toy");
        toyScript = (ToyScript)toy.GetComponent(typeof(ToyScript));
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100)) {
                if (hit.collider.gameObject.name == "PlayButton") {
                    Renderer hitRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                    if (toyScript.PathPointsPresent()) {
                        if (hitRenderer.material.color == Color.red) {
                            hitRenderer.material.color = Color.green;
                            toyScript.DebugShowPathPoints();
                            toyScript.StartPlaying();
                        } else {
                            hitRenderer.material.color = Color.red;
                            toyScript.StopPlaying();
                        }
                    }
                    else {
                        Debug.Log("No animation has been recorded");
                    }
                }
            }
        }
    }

}