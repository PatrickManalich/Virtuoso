using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyScript : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private bool playing;
    private float startTime;
    private List<Vector3> samplePosList;
    private List<Quaternion> sampleRotList;
    public float sampleRate;
    public GameObject samplePrefab;
    public GameObject playSlider;
    private PlaySliderScript playSliderScript;
    private int lastTargetIndex;
    private float lastT;
    private Vector3 lastBaseSamplePos;
    private Quaternion lastBaseSampleRot;
    private bool paused;

    void Start () {
        samplePosList = new List<Vector3>();
        sampleRotList = new List<Quaternion>();
        playSliderScript = playSlider.GetComponent<PlaySliderScript>();
        lastTargetIndex = 1;
        lastT = 0f;
        lastBaseSamplePos = transform.position;
        lastBaseSampleRot = transform.rotation;
        paused = false;
    }

    void Update() { }

    void OnMouseDown() {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        startTime = Time.time;
        samplePosList.Add(transform.position);
        sampleRotList.Add(transform.rotation);
    }

    void OnMouseDrag() {
        Vector3 currScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 currPosition = Camera.main.ScreenToWorldPoint(currScreenPoint) + offset;
        transform.position = currPosition;
        if((Time.time - startTime) > sampleRate) {
            startTime = Time.time;
            samplePosList.Add(transform.position);
            sampleRotList.Add(transform.rotation);
        }

    }

    void OnMouseUp() {
        samplePosList.Add(transform.position);
        sampleRotList.Add(transform.rotation);
        transform.position = samplePosList[0];
        transform.rotation = sampleRotList[0];
    }

    public IEnumerator MoveToNextSample() {
        if (lastTargetIndex < samplePosList.Count) {
            if (!paused) {
                lastBaseSamplePos = transform.position;
                lastBaseSampleRot = transform.rotation;
                lastT = 0f;
            } else
                paused = false;
            while (lastT < 1) {
                lastT += Time.deltaTime / sampleRate;
                transform.position = Vector3.Lerp(lastBaseSamplePos, samplePosList[lastTargetIndex], lastT);
                transform.rotation = Quaternion.Lerp(lastBaseSampleRot, sampleRotList[lastTargetIndex], lastT);
                float startingSamplePercent = ((float) lastTargetIndex - 1.0f) / ((float)samplePosList.Count - 1.0f);
                float sliderPercent = lastT * (1.0f / ((float) samplePosList.Count - 1.0f)) + startingSamplePercent;
                playSliderScript.CalibrateWithToy(sliderPercent);
                yield return null;
            }
            lastTargetIndex++;
            yield return MoveToNextSample();
        } else {
            transform.position = samplePosList[0];
            transform.rotation = sampleRotList[0];
            lastBaseSamplePos = transform.position;
            lastBaseSampleRot = transform.rotation;
            lastTargetIndex = 1;
            yield return MoveToNextSample();
        }
    }

    public bool AnimationRecorded() {
        return samplePosList.Count > 0;
    }

    public void CalibrateWithPlaySlider(float sliderPercent) {
        if (sliderPercent < 0f)
            sliderPercent = 0f;
        else if (sliderPercent > 1f)
            sliderPercent = 1f;
        int startingSampleIndex = (int)(sliderPercent * (samplePosList.Count - 1)); // Casting automatically floors
        lastTargetIndex = startingSampleIndex;
        float startingSamplePercent = (float) startingSampleIndex / ((float) samplePosList.Count - 1.0f);
        float t = (sliderPercent - startingSamplePercent) / (1.0f / ((float) samplePosList.Count - 1.0f));
        transform.position = Vector3.Lerp(samplePosList[startingSampleIndex], samplePosList[startingSampleIndex+1], t);
        transform.rotation = Quaternion.Lerp(sampleRotList[startingSampleIndex], sampleRotList[startingSampleIndex + 1], t);
    }

    public Vector3 GetPositionAtSliderPercent(float sliderPercent) {
        if (sliderPercent < 0f)
            sliderPercent = 0f;
        else if (sliderPercent > 1f)
            sliderPercent = 1f;
        int startingSampleIndex = (int)(sliderPercent * (samplePosList.Count - 1)); // Casting automatically floors
        if (startingSampleIndex == samplePosList.Count - 1)
            return samplePosList[samplePosList.Count - 1];
        else {
            float startingSamplePercent = (float)startingSampleIndex / ((float)samplePosList.Count - 1.0f);
            float t = (sliderPercent - startingSamplePercent) / (1.0f / ((float)samplePosList.Count - 1.0f));
            return Vector3.Lerp(samplePosList[startingSampleIndex], samplePosList[startingSampleIndex + 1], t);
        }     
    }

    public Quaternion GetRotationAtSliderPercent(float sliderPercent) {
        if (sliderPercent < 0f)
            sliderPercent = 0f;
        else if (sliderPercent > 1f)
            sliderPercent = 1f;
        int startingSampleIndex = (int)(sliderPercent * (samplePosList.Count - 1)); // Casting automatically floors
        if (startingSampleIndex == samplePosList.Count - 1)
            return sampleRotList[samplePosList.Count - 1];
        else {
            float startingSamplePercent = (float)startingSampleIndex / ((float)samplePosList.Count - 1.0f);
            float t = (sliderPercent - startingSamplePercent) / (1.0f / ((float)samplePosList.Count - 1.0f));
            return Quaternion.Lerp(sampleRotList[startingSampleIndex], sampleRotList[startingSampleIndex + 1], t);
        }
    }

    public void StartPlaying() {
        StartCoroutine(MoveToNextSample());
    }

    public void StopPlaying() {
        StopAllCoroutines();
        paused = true;
    }

    //-------------------------------------------------------------------------------------

    public void DebugInstantiateSamples() {
        for (int i = 0; i < samplePosList.Count; i++) {
            Instantiate(samplePrefab, samplePosList[i], sampleRotList[i]);
        }
    }

    public void DebugDestroySamples() {
        GameObject[] samples = GameObject.FindGameObjectsWithTag("Sample");
        foreach (GameObject sample in samples) {
            Destroy(sample);
        }
    }

    public void DebugAddSample(Vector3 samplePos, Quaternion sampleRot) {
        samplePosList.Add(samplePos);
        sampleRotList.Add(sampleRot);
    }

    public void DebugReset() {
        transform.position = samplePosList[0];
        transform.rotation = sampleRotList[0];
    }

}
