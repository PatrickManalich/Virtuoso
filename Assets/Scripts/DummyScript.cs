using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour {

    private int sampleCount;                        // The number of samples in the animation
    private SpeedBandScript speedBandScript;        // The speed band script, used for caching
    private PlayBandScript playBandScript;          // The play band script, used for caching
    private SliderFieldScript sliderFieldScript;    // The slider field script, used for caching
    private Vector3[] samplePositions;              // An array of the sample positions
    private Quaternion[] sampleRotations;           // An array of the sample rotations
    private int lastSampleIndex;                    // The index of the last sample, used for playing animation
    private float lastT;                            // The last t, used for playing animation
    private bool isPaused;                          // Has the animation been paused
    private Vector3[] editedSamplePositions;        // An array of the edited sample positions
    private Quaternion[] editedSampleRotations;     // An array of the edited sample rotations

    public float animationSeconds;                  // How long the animation lasts in seconds
    public GameObject speedBand;                    // The speed band of the slider field
    public GameObject playBand;                     // The play band of the slider field
    public GameObject sliderField;                  // The slider field Game Object, used for adjusting current slider

    private void Awake() {
        transform.position = new Vector3(0f, -0.46f, 0.75f);
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        speedBandScript = speedBand.GetComponent<SpeedBandScript>();
        playBandScript = playBand.GetComponent<PlayBandScript>();
        sliderFieldScript = sliderField.GetComponent<SliderFieldScript>();
            // Initialize sample positions and sample rotations
        sampleCount = (int) Mathf.Ceil(animationSeconds / speedBandScript.GetSampleRate()); // Ceil to make nice even number
        samplePositions = new Vector3[sampleCount];
        for(int i = 0; i < sampleCount; i++) { samplePositions[i] = transform.position; }
        sampleRotations = new Quaternion[sampleCount];
        for (int i = 0; i < sampleCount; i++) { sampleRotations[i] = transform.rotation; }

        lastSampleIndex = (sampleCount / 2) - 1;
        lastT = 0f;
        isPaused = false;

        editedSamplePositions = new Vector3[sampleCount];
        editedSampleRotations = new Quaternion[sampleCount];
    }

    private void Start() {
        sliderFieldScript.AdjustSlider("StartSlider", 0, 0f);
        sliderFieldScript.AdjustSlider("CurrentSlider", lastSampleIndex, 0f);
        sliderFieldScript.AdjustSlider("EndSlider", sampleCount - 1, 0f);
    }

    /* Coroutine that moves the dummy from its current sample to the next sample by lerping over t. Will move back to first sample after
     * it has reached the last sample. Stores the information in global variables lastT and lastSampleIndex for the ability to pause and
     * then play again from the same position. */
    private IEnumerator MoveToNextSample() {
        if (lastSampleIndex < sliderFieldScript.GetEndSliderSampleIndex()) {    // If last sample index isn't out of range, continue playing
            if (!isPaused)
                lastT = 0f;
            else
                isPaused = false;
            while (lastT < 1) { // Lerp from t being 0 to t being 1
                lastT += Time.deltaTime / speedBandScript.GetSampleRate();
                transform.position = Vector3.Lerp(samplePositions[lastSampleIndex], samplePositions[lastSampleIndex + 1], lastT);
                transform.rotation = Quaternion.Lerp(sampleRotations[lastSampleIndex], sampleRotations[lastSampleIndex + 1], lastT);
                sliderFieldScript.AdjustSlider("CurrentSlider", lastSampleIndex, lastT);
                yield return null;
            }
                // When it has reached the next sample, increment last sample index and start MoveToNextSample again
            lastSampleIndex++;
            yield return StartCoroutine(MoveToNextSample());
        } else {    // If the last sample index is out of range, reset dummy to start slider positin and start MoveToNextSample again
            GoToStart();
            yield return StartCoroutine(MoveToNextSample());
        }
    }

    /* */
    private IEnumerator OverwriteNextSample() {
        if (lastSampleIndex <= sliderFieldScript.GetEndSliderSampleIndex()) {
            editedSamplePositions[lastSampleIndex] = transform.position;
            editedSampleRotations[lastSampleIndex] = transform.rotation;
            sliderFieldScript.AdjustSlider("CurrentSlider", lastSampleIndex, 0f);
            lastSampleIndex++;
            yield return new WaitForSeconds(speedBandScript.GetSampleRate());
            yield return StartCoroutine(OverwriteNextSample());
        } else
            yield return null;
    }

    /* Returns the sample count. */
    public int GetSampleCount() { return sampleCount; }

    /* Takes in a sample index and returns the sample position at that index. */
    public Vector3 GetSamplePosition(int sampleIndex) { return samplePositions[sampleIndex]; }

    /* Takes in a sample index and returns the sample rotation at that index. */
    public Quaternion GetSampleRotation(int sampleIndex) { return sampleRotations[sampleIndex]; }

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

    /* Place dummy at the start slider position. */
    public void GoToStart() {
        Adjust(sliderFieldScript.GetStartSliderSampleIndex(), 0f);
        sliderFieldScript.AdjustSlider("CurrentSlider", sliderFieldScript.GetStartSliderSampleIndex(), 0f);
    }

    /* */
    public void GrabBegin() {
        if (!isPaused) {    // If it was playing when grabbed, stop playing and force the play band to pause
            StopPlaying();
            playBandScript.ForcePauseToggle();
        }
        sliderFieldScript.AdjustSlider("StartSlider", lastSampleIndex, 0f);     // force the start slider to align
        lastT = 0f;
        StartCoroutine(OverwriteNextSample());
    }

    /* */
    public void GrabEnd() {
        StopAllCoroutines();
        for (int i = sliderFieldScript.GetStartSliderSampleIndex(); i < sliderFieldScript.GetEndSliderSampleIndex(); i++) {
            samplePositions[i] = editedSamplePositions[i];
            sampleRotations[i] = editedSampleRotations[i];
        }
        sliderFieldScript.AdjustSlider("EndSlider", lastSampleIndex - 1, 0f);   // force the end slider to align, the minus one comes from
                                                                                // lastSampleIndex incrementing an extra time on last sample    
        GoToStart();
    }

}
