using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour {

    private int sampleCount;                        // The number of samples in the animation
    private SpeedBandScript speedBandScript;        // The speed band script, used for caching
    private SliderFieldScript sliderFieldScript;    // The slider field script, used for caching 
    private float lastSampleTakenTime;              // The time at which the last sample had been taken
    private Vector3[] samplePositions;              // An array of the sample positions
    private Quaternion[] sampleRotations;           // An array of the sample rotations
    private int lastSampleIndex;                    // The index of the last sample, used for playing animation
    private float lastT;                            // The last t, used for playing animation
    private bool isPaused;                          // Has the animation been paused

    public float animationSeconds;                  // How long the animation lasts in seconds
    public GameObject speedBand;                    // The speed band of the slider field
    public GameObject sliderField;                  // The slider field

    private void Awake() {
        speedBandScript = speedBand.GetComponent<SpeedBandScript>();
        sliderFieldScript = sliderField.GetComponent<SliderFieldScript>();
            // Initialize sample positions and sample rotations
        sampleCount = (int) Mathf.Ceil(animationSeconds / speedBandScript.GetSampleRate()); // Ceil to make nice even number
        samplePositions = new Vector3[sampleCount];
        //for(int i = 0; i < sampleCount; i++) { samplePositions[i] = transform.position; }
        float increment = 0.0f; // DELETE
        for (int i = 0; i < sampleCount; i++) { samplePositions[i] = V3E.SetX(transform.position,increment); increment += 0.01f; }  // DELETE
        sampleRotations = new Quaternion[sampleCount];
        //for (int i = 0; i < sampleCount; i++) { sampleRotations[i] = transform.rotation; }
        int incrementRot = 0;   // DELETE
        for (int i = 0; i < sampleCount; i++) { sampleRotations[i] = transform.rotation * Quaternion.Euler(0, incrementRot, 0); incrementRot++; }   // DELETE

        lastSampleIndex = 0;
        lastT = 0f;
        isPaused = false;
    }

    private void Start() { // DELETE
        //StartPlaying();
    }

    /* Coroutine that moves the dummy from its current sample to the next sample by lerping over t. Will move back to first sample after
     * it has reached the last sample. Stores the information in global variables lastT and lastSampleIndex for the ability to pause and
     * then play again from the same position. */
    private IEnumerator MoveToNextSample() {
        if (lastSampleIndex < samplePositions.Length - 1) {
            if (!isPaused)
                lastT = 0f;
            else
                isPaused = false;
            while (lastT < 1) {
                lastT += Time.deltaTime / speedBandScript.GetSampleRate();
                transform.position = Vector3.Lerp(samplePositions[lastSampleIndex], samplePositions[lastSampleIndex + 1], lastT);
                transform.rotation = Quaternion.Lerp(sampleRotations[lastSampleIndex], sampleRotations[lastSampleIndex + 1], lastT);
                sliderFieldScript.AdjustCurrentSlider(lastSampleIndex, lastT);
                yield return null;
            }
            lastSampleIndex++;
            yield return MoveToNextSample();
        } else {
            transform.position = samplePositions[0];
            transform.rotation = sampleRotations[0];
            lastSampleIndex = 0;
            lastT = 0f;
            yield return MoveToNextSample();
        }
    }

    /* Returns the sample count. */
    public int GetSampleCount() { return sampleCount; }

    /* Takes in a sample index and a t and adjusts the position of the dummy based on the values. */
    public void Adjust(int sampleIndex, float t) {
        lastT = t;
        lastSampleIndex = sampleIndex;
        transform.position = Vector3.Lerp(samplePositions[lastSampleIndex], samplePositions[lastSampleIndex + 1], lastT);
        transform.rotation = Quaternion.Lerp(sampleRotations[lastSampleIndex], sampleRotations[lastSampleIndex + 1], lastT);
    }

    /* Begin playing the animation by starting the MoveToNextSample() coroutine. */
    public void StartPlaying() { StartCoroutine(MoveToNextSample()); }

    /* Stop playing the animation by stopping the coroutine. */
    public void StopPlaying() {
        StopAllCoroutines();
        isPaused = true;
    }

    /* */
    public void GrabBegin() {
        Debug.Log("grab begin");
    }    

    /* *.
    public void GrabEnd() {
        Debug.Log("grab end");
    }

}
