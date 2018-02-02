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

    void Start () {
        samplePosList = new List<Vector3>();
        sampleRotList = new List<Quaternion>();
        playSliderScript = playSlider.GetComponent<PlaySliderScript>();
        lastTargetIndex = 1;
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
    }

    public IEnumerator MoveToPosition(int targetIndex, float sampleRate) {
        if (targetIndex < samplePosList.Count) {
            float t = 0f;
            while (t < 1) {
                t += Time.deltaTime / sampleRate;
                transform.position = Vector3.Lerp(transform.position, samplePosList[targetIndex], t);
                transform.rotation = Quaternion.Lerp(transform.rotation, sampleRotList[targetIndex], t);
                float startingSamplePercent = ((float) targetIndex - 1.0f) / ((float)samplePosList.Count - 1.0f);
                float sliderPercent = t * (1.0f / ((float) samplePosList.Count - 1.0f)) + startingSamplePercent;
                if (sliderPercent > 1.0f)
                    sliderPercent = 1.0f;
                playSliderScript.MatchToyPosition(sliderPercent);
                yield return null;
            }
            lastTargetIndex = targetIndex + 1;
            yield return MoveToPosition(targetIndex + 1, sampleRate);
        } else {
            transform.position = samplePosList[0];
            transform.rotation = sampleRotList[0];
            yield return MoveToPosition(1, sampleRate);
        }
    }

    public bool AnimationRecorded() {
        return samplePosList.Count > 0;
    }

    public void MatchSliderPosition(float sliderPercent) {
        int startingSampleIndex = (int)(sliderPercent * (samplePosList.Count - 1)); // Casting automatically floors
        lastTargetIndex = startingSampleIndex;
        float startingSamplePercent = (float) startingSampleIndex / ((float) samplePosList.Count - 1.0f);
        float t = (sliderPercent - startingSamplePercent) / (1.0f / ((float) samplePosList.Count - 1.0f));
        transform.position = Vector3.Lerp(samplePosList[startingSampleIndex], samplePosList[startingSampleIndex+1], t);
        transform.rotation = Quaternion.Lerp(sampleRotList[startingSampleIndex], sampleRotList[startingSampleIndex + 1], t);
    }

    public void StartPlaying() {
        StartCoroutine(MoveToPosition(lastTargetIndex, sampleRate));
    }

    public void StopPlaying() {
        StopAllCoroutines();
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

}
