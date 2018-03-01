using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour {

    private float startTime;
    private List<Vector3> viewSamplePosList;
    private List<Quaternion> viewSampleRotList;
    private int lastSampleIndex;
    private float lastT;
    private bool isPaused;
    
    public GameObject samplePrefab;
    public bool isInEditMode;
    public bool isAnimationRecorded;


    private void Awake() {
        viewSamplePosList = new List<Vector3>();
        viewSampleRotList = new List<Quaternion>();
        lastSampleIndex = 0;
        lastT = 0f;
        isPaused = false;
        isAnimationRecorded = false;
    }

    public void GrabBegin() {

    }
    

    public void GrabEnd() {

    }

}
