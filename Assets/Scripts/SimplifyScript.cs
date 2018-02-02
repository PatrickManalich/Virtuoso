using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplifyScript : MonoBehaviour {

    public GameObject toy;
    private ToyScript toyScript;
    //private GameObject controller;
    //private ControllerScript controllerScript;
    public float smooth = 2.0F;
    public float tiltAngle = 30.0F;

    void Start () {
        toyScript = toy.GetComponent<ToyScript>();
        //controller = GameObject.Find("Controller");
        //controllerScript = (ControllerScript)controller.GetComponent(typeof(ControllerScript));

        //Vector3 pathPoint0 = new Vector3(-1f, 0.5f, -1.62f);
        //Vector3 pathPoint1 = new Vector3(0f, 0.5f, -1.62f);
        //Vector3 pathPoint2 = new Vector3(1f, 0.5f, -1.62f);
        //toyScript.DebugAddPathPoint(pathPoint0);
        //toyScript.DebugAddPathPoint(pathPoint1);
        //toyScript.DebugAddPathPoint(pathPoint2);
        //toyScript.DebugShowPathPoints();
        //toyScript.StartPlaying();
    }

    void Update () {
        if (Input.GetKeyDown("space")) {
            //StopAllCoroutines();
            toyScript.StopPlaying();
        }

        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
        toy.transform.rotation = Quaternion.Slerp(toy.transform.rotation, target, Time.deltaTime * smooth);
    }

}
