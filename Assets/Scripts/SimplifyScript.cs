using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplifyScript : MonoBehaviour {

    private GameObject toy;
    private ToyScript toyScript;
    //private GameObject controller;
    //private ControllerScript controllerScript;

	void Start () {
        toy = GameObject.Find("Toy");
        toyScript = (ToyScript)toy.GetComponent(typeof(ToyScript));
        //controller = GameObject.Find("Controller");
        //controllerScript = (ControllerScript)controller.GetComponent(typeof(ControllerScript));

        Vector3 pathPoint0 = new Vector3(-1f, 0.5f, -1.62f);
        Vector3 pathPoint1 = new Vector3(0f, 0.5f, -1.62f);
        Vector3 pathPoint2 = new Vector3(1f, 0.5f, -1.62f);
        toyScript.DebugAddPathPoint(pathPoint0);
        toyScript.DebugAddPathPoint(pathPoint1);
        toyScript.DebugAddPathPoint(pathPoint2);
        toyScript.DebugShowPathPoints();
        //toyScript.StartPlaying();
    }

    void Update () {
        if (Input.GetKeyDown("space")) {
            //StopAllCoroutines();
            toyScript.StopPlaying();
        }
    }

}
