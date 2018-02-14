using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyScript : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private float startTime;
    private List<Vector3> samplePosList;
    private List<Quaternion> sampleRotList;
    public GameObject samplePrefab;
    public GameObject playSlider;
    private PlaySliderScript playSliderScript;
    private int lastSampleIndex;
    private float lastT;
    private bool isPaused;
    public bool isInEditMode;
    public bool isAnimationRecorded;
    public GameObject editGhostToy;
    private int beginSampleIndex;
    private int endSampleIndex;
    private List<Vector3> editSamplePosList;
    private List<Quaternion> editSampleRotList;
    public GameObject speedButton;
    private SpeedButtonScript speedButtonScript;

    void Awake () {
        samplePosList = new List<Vector3>();
        sampleRotList = new List<Quaternion>();
        editSamplePosList = new List<Vector3>();
        editSampleRotList = new List<Quaternion>();
        playSliderScript = playSlider.GetComponent<PlaySliderScript>();
        speedButtonScript = speedButton.GetComponent<SpeedButtonScript>();
        lastSampleIndex = 0;
        lastT = 0f;
        isPaused = false;
        isAnimationRecorded = false;
    }

    void OnMouseDown() {
        if (!isAnimationRecorded || isInEditMode) {
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            startTime = Time.time;
            if (isInEditMode) {
                editSamplePosList.Clear();
                editSampleRotList.Clear();
            } else {
                samplePosList.Add(transform.position);
                sampleRotList.Add(transform.rotation);
            }        
        } else            
            Debug.Log("Please move to edit mode for editing");
    }

    void OnMouseDrag() {
        if(!isAnimationRecorded || isInEditMode) {
            Vector3 currScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 currPosition = Camera.main.ScreenToWorldPoint(currScreenPoint) + offset;
            transform.position = currPosition;
            if ((Time.time - startTime) > speedButtonScript.GetSampleRate()) {
                startTime = Time.time;
                if (isInEditMode) {
                    //Debug.Log(samplePosList.Coun);
                    if(editSamplePosList.Count < (endSampleIndex - beginSampleIndex - 1)) {
                        editSamplePosList.Add(transform.position);
                        editSampleRotList.Add(transform.rotation);
                        //Debug.Log("added");
                    }                  
                } else {
                    samplePosList.Add(transform.position);
                    sampleRotList.Add(transform.rotation);
                }
            }
        }        
    }

    void OnMouseUp() {
        if (!isAnimationRecorded || isInEditMode) {
            if (isInEditMode) {
                transform.position = samplePosList[beginSampleIndex];
                transform.rotation = sampleRotList[beginSampleIndex];
                lastSampleIndex = beginSampleIndex;
                lastT = 0f;
                for(int i = beginSampleIndex + 1, j = 0; i < endSampleIndex; i++, j++) {
                    samplePosList[i] = editSamplePosList[j];
                    sampleRotList[i] = editSampleRotList[j];
                }
            }
            else {
                samplePosList.Add(transform.position);
                sampleRotList.Add(transform.rotation);
                transform.position = samplePosList[0];
                transform.rotation = sampleRotList[0];
                isAnimationRecorded = true;
            }
        }
    }

    public IEnumerator MoveToNextSample() {
        if (lastSampleIndex < samplePosList.Count - 1) {
            if (!isPaused)
                lastT = 0f;
            else
                isPaused = false;
            while (lastT < 1) {
                lastT += Time.deltaTime / speedButtonScript.GetSampleRate();
                transform.position = Vector3.Lerp(samplePosList[lastSampleIndex], samplePosList[lastSampleIndex + 1], lastT);
                transform.rotation = Quaternion.Lerp(sampleRotList[lastSampleIndex], sampleRotList[lastSampleIndex + 1], lastT);
                playSliderScript.CalibrateWithToyLocation(lastSampleIndex, lastT);
                yield return null;
            }
            lastSampleIndex++;
            yield return MoveToNextSample();
        } else {
            transform.position = samplePosList[0];
            transform.rotation = sampleRotList[0];
            lastSampleIndex = 0;
            lastT = 0f;
            yield return MoveToNextSample();
        }
    }

    public void ChangeLocation(int startingSampleIndex, float t) {
        lastT = t;
        lastSampleIndex = startingSampleIndex;
        transform.position = Vector3.Lerp(samplePosList[lastSampleIndex], samplePosList[lastSampleIndex + 1], lastT);
        transform.rotation = Quaternion.Lerp(sampleRotList[lastSampleIndex], sampleRotList[lastSampleIndex + 1], lastT);
    }

    public int GetSampleCount() { return samplePosList.Count; }

    public Vector3 GetSamplePos(int sampleIndex) { return samplePosList[sampleIndex]; }

    public Quaternion GetSampleRot(int sampleIndex) { return sampleRotList[sampleIndex]; }

    public int GetLastSampleIndex() { return lastSampleIndex; }

    public void PRINTYO() {
        Debug.Log("PRINTYO");
    }

    public void StartPlaying() { StartCoroutine(MoveToNextSample()); }

    public void StopPlaying() {
        StopAllCoroutines();
        isPaused = true;
    }

    public IEnumerator MoveGhostToNextSample(int ghostSampleIndex) {
        if (ghostSampleIndex < endSampleIndex) {
            float t = 0f;
            while (t < 1) {
                t += Time.deltaTime / speedButtonScript.GetSampleRate();
                editGhostToy.transform.position = Vector3.Lerp(samplePosList[ghostSampleIndex], samplePosList[ghostSampleIndex + 1], t);
                editGhostToy.transform.rotation = Quaternion.Lerp(sampleRotList[ghostSampleIndex], sampleRotList[ghostSampleIndex + 1], t);
                yield return null;
            }
            yield return MoveGhostToNextSample(ghostSampleIndex + 1);
        } else {
            editGhostToy.transform.position = samplePosList[beginSampleIndex];
            editGhostToy.transform.rotation = sampleRotList[beginSampleIndex];
            yield return MoveGhostToNextSample(beginSampleIndex);
        }
    }

    public void StartGhosting(int beginSampleIndexParam, int endSampleIndexParam) {
        editGhostToy.SetActive(true);
        beginSampleIndex = beginSampleIndexParam;
        endSampleIndex = endSampleIndexParam;
        editGhostToy.transform.position = samplePosList[beginSampleIndex];
        editGhostToy.transform.rotation = sampleRotList[beginSampleIndex];
        StartCoroutine(MoveGhostToNextSample(beginSampleIndex));
    }

    public void StopGhosting() {
        editGhostToy.SetActive(false);
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

    public void DebugAddSample(Vector3 samplePos, Quaternion sampleRot) {
        samplePosList.Add(samplePos);
        sampleRotList.Add(sampleRot);
    }

    public void DebugReset() {
        transform.position = samplePosList[0];
        transform.rotation = sampleRotList[0];
    }

}
