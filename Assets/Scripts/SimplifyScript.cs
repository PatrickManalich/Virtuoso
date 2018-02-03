using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplifyScript : MonoBehaviour {

    // The SimplifyScript.cs must be inactive and then turned active in Unity editor after initialization of Toy
    // so a NullReferenceException isn't thrown

    public GameObject toy;
    private ToyScript toyScript;
    public float smooth = 2.0F;
    public float tiltAngle = 30.0F;
    Vector3 sample0;
    Vector3 sample1;
    Vector3 sample2;
    Vector3 sample3;

    void Start () {
        toyScript = toy.GetComponent<ToyScript>();

        sample0 = new Vector3(-2f, 0.5f, -1.62f);
        sample1 = new Vector3(0f, 0.5f, -1.62f);
        sample2 = new Vector3(2f, 0.5f, -1.62f);
        sample3 = new Vector3(3f, 0.5f, -1.62f);
        //toyScript.DebugAddSample(sample0, Quaternion.identity);
        //toyScript.DebugAddSample(sample1, Quaternion.identity);
        //toyScript.DebugAddSample(sample2, Quaternion.identity);
        //toyScript.DebugAddSample(sample3, Quaternion.identity);
        //toyScript.DebugInstantiateSamples();
        //toyScript.DebugReset();
        //toyScript.StartPlaying();
    }

    void Update () {
        if (Input.GetKeyDown("space")) {
            toyScript.DebugDestroySamples();
            toyScript.StopPlaying();
        }

        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
        toy.transform.rotation = Quaternion.Slerp(toy.transform.rotation, target, Time.deltaTime * smooth);
    }

}
