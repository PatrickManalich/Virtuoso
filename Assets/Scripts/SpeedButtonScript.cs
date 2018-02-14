using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedButtonScript : MonoBehaviour {

    private enum Speed { RealTime, Slower, Faster };
    private Speed speedMode;
    public float sampleRate;
    public GameObject speedButtonText;
    
    void Awake() { speedMode = Speed.RealTime; }

    void OnMouseDown() {
        if(speedMode == Speed.RealTime) {
            speedMode = Speed.Slower;
            speedButtonText.GetComponent<TextMesh>().text = "  Slower";
        } else if(speedMode == Speed.Slower) {
            speedMode = Speed.Faster;
            speedButtonText.GetComponent<TextMesh>().text = "  Faster";
        } else {
            speedMode = Speed.RealTime;
            speedButtonText.GetComponent<TextMesh>().text = "Real-Time";
        }
    }

    public float GetSampleRate() {
        if (speedMode == Speed.RealTime)
            return sampleRate;
        else if (speedMode == Speed.Slower)
            return sampleRate * 2f;
        else
            return sampleRate * 0.5f;        
    }


}