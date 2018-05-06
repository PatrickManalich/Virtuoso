using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DummyScript : MonoBehaviour {

    private DummyManagerScript DMS;                 // The dummy manager script
    private int sampleCount;                        // The number of samples in the animation
    private Vector3[] samplePositions;              // An array of the sample positions
    private Quaternion[] sampleRotations;           // An array of the sample rotations
    private int lastSampleIndex;                    // The index of the last sample, used for playing animation
    private float lastT;                            // The last t, used for playing animation
    private bool isPaused;                          // Has the animation been paused
    private Vector3[] alternativeSamplePositions;   // Stores the alternative sample positions used for undoing and redoing animations
    private Quaternion[] alternativeSampleRotations;    // Stors the alternative sample rotations used for undoing and redoing animations

    /* Coroutine that moves the dummy from its current sample to the next sample by lerping over t. Will move back to first sample after
     * it has reached the last sample. Stores the information in global variables lastT and lastSampleIndex for the ability to pause and
     * then play again from the same position. */
    private IEnumerator MoveToNextSample() {
        if (lastSampleIndex < DMS.SFS_GetEndSliderSampleIndex()) {    // If last sample index isn't out of range, continue playing
            if (!isPaused)
                lastT = 0f;
            else
                isPaused = false;
            while (lastT < 1) { // Lerp from t being 0 to t being 1
                lastT += Time.deltaTime / DMS.SBS_GetSampleRate();
                transform.position = Vector3.Lerp(samplePositions[lastSampleIndex], samplePositions[lastSampleIndex + 1], lastT);
                transform.rotation = Quaternion.Lerp(sampleRotations[lastSampleIndex], sampleRotations[lastSampleIndex + 1], lastT);
                DMS.SFS_AdjustSlider("CurrentSlider", lastSampleIndex, lastT);
                yield return null;
            }
                // When it has reached the next sample, increment last sample index and start MoveToNextSample again
            lastSampleIndex++;
            yield return StartCoroutine(MoveToNextSample());
        } else {    // If the last sample index is out of range, reset dummy to start slider position and start MoveToNextSample again
            GoToStart();
            yield return StartCoroutine(MoveToNextSample());
        }
    }

    /* Coroutine that overwrites the samples every sample rate seconds. Will update and used last sample index to recursively call itself
     * until it has reached the end slider sample index, in which case it will force release the left and right grabber. */
    private IEnumerator OverwriteNextSample() {
        if (lastSampleIndex <= DMS.SFS_GetEndSliderSampleIndex()) {
            alternativeSamplePositions[lastSampleIndex] = samplePositions[lastSampleIndex];
            alternativeSampleRotations[lastSampleIndex] = sampleRotations[lastSampleIndex];
            samplePositions[lastSampleIndex] = transform.position;
            sampleRotations[lastSampleIndex] = transform.rotation;
            DMS.SFS_AdjustSlider("CurrentSlider", lastSampleIndex, 0f);
            lastSampleIndex++;
            yield return new WaitForSeconds(DMS.SBS_GetSampleRate());
            yield return StartCoroutine(OverwriteNextSample());
        } else {
            DMS.OVRG_ForceRelease(gameObject.GetComponent<OVRGrabbable>());
            yield return null;
        }
    }

    /* Refines each sample index in between the start slider sample index and the end slider sample index. Uses a Hermite curve to give
     * ease-in ease-out functionality. Slices the work into two halves, one from the start slider sample index to the last sample index,
     * and one from the last sample index to the end sample index. */
    private void ApplyRefinement() {
        float percent;
        int startSliderSampleIndex = DMS.SFS_GetStartSliderSampleIndex(); // For caching
        int endSliderSampleIndex = DMS.SFS_GetEndSliderSampleIndex();     // For caching

        Vector3 diff = transform.position - samplePositions[lastSampleIndex];
        // From range [start slider sample index, last sample index]
        for (int i = startSliderSampleIndex; i <= lastSampleIndex; i++) {
            percent = (i - startSliderSampleIndex) / ((float)lastSampleIndex - startSliderSampleIndex);
            float hermitePercent = Mathfx.Hermite(0.0f, 1.0f, percent);
            Vector3 offset = diff * hermitePercent;
            alternativeSamplePositions[i] = samplePositions[i];
            samplePositions[i] = samplePositions[i] + offset;
        }

        diff = transform.position - samplePositions[lastSampleIndex + 1];
        // From range (last sample index, end slider sample index], going backwards
        for (int i = endSliderSampleIndex; i >= lastSampleIndex + 1; i--) {
            percent = (endSliderSampleIndex - i) / (float)(endSliderSampleIndex - lastSampleIndex - 1);
            float hermitePercent = Mathfx.Hermite(0.0f, 1.0f, percent);
            Vector3 offset = diff * hermitePercent;
            alternativeSamplePositions[i] = samplePositions[i];
            samplePositions[i] = samplePositions[i] + offset;
        }
    }

    /* Takes in a dummy manager script and a sample count param, and initializes its private variables with these parameters. Also 
     * initializes sample positions, sample rotations, alternative sample positions, alternative sample rotations, and other private
     * variables. */
    public void Initialize(ref DummyManagerScript DMSParam, int sampleCountParam) {
        
        DMS = DMSParam;
        sampleCount = sampleCountParam;

        samplePositions = new Vector3[sampleCount];
        for (int i = 0; i < sampleCount; i++) { samplePositions[i] = transform.position; }
        sampleRotations = new Quaternion[sampleCount];
        for (int i = 0; i < sampleCount; i++) { sampleRotations[i] = transform.rotation; }

        alternativeSamplePositions = new Vector3[sampleCount];
        for (int i = 0; i < sampleCount; i++) { alternativeSamplePositions[i] = transform.position; }
        alternativeSampleRotations = new Quaternion[sampleCount];
        for (int i = 0; i < sampleCount; i++) { alternativeSampleRotations[i] = transform.rotation; }

        lastSampleIndex = (sampleCount / 2) - 1;
        lastT = 0f;
        isPaused = false;
    }

    /* Takes in a sample index and returns the sample position at that index. */
    public Vector3 GetSamplePosition(int sampleIndex) { return samplePositions[sampleIndex]; }

    /* Takes in a sample index and returns the sample rotation at that index. */
    public Quaternion GetSampleRotation(int sampleIndex) { return sampleRotations[sampleIndex]; }

    /* Returns the last sample index of this dummy. */
    public int GetLastSampleIndex() { return lastSampleIndex; }

    /* Returns the last t of this dummy. */
    public float GetLastT() { return lastT; }

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

    /* Sends the dummy to the start slider sample */
    public void GoToStart() { Adjust(DMS.SFS_GetStartSliderSampleIndex(), 0f); }

    /* Called once when the dummy is first grabbed. Depending on the state of the left glove (overwite or refine), will call the appropriate
     * functions. */
    public void GrabBegin() {
        if (!isPaused) {    // If it was playing when grabbed, stop playing and force the play band to pause
            StopPlaying();
            DMS.PBS_ForceIntoPauseToggleState();
        }
        DMS.UB_ForceIntoUndoToggleState();

        if (DMS.LGS_IsInOverwriteState()) {
            DMS.SFS_AdjustSlider("StartSlider", lastSampleIndex, 0f);     // force the start slider to align
            DMS.AdjustStartAids(lastSampleIndex);
            lastT = 0f;
            StartCoroutine(OverwriteNextSample());
        } else    // Refining
            DMS.RGS_StartGuiding(transform.position);
    }

    /* Called once the dummy is let go. Depending on the state of the left glove (overwite or refine), will either overwrite samples with
     * edited samples or call ApplyRefinement(). */
    public void GrabEnd() {
        if (DMS.LGS_IsInOverwriteState()) {
            StopAllCoroutines();
            DMS.SFS_AdjustSlider("EndSlider", lastSampleIndex - 1, 0f);   // force the end slider to align, the minus one comes from lastSampleIndex 
                                                                          // incrementing an extra time on last sample 
            DMS.AdjustEndAids(lastSampleIndex - 1);
            DMS.DS_GoToStart();
        } else {    // Refining
            DMS.RGS_StopGuiding();
            ApplyRefinement();
        }
    }


    /* Swaps all sample positions and rotations from the current array to the alternative array, from start slider sample index to
     * end slider sample index. Used for undoing and redoing. */
    public void AlternateSamples() {
        Vector3 tempPosition;
        Quaternion tempRotation;
        for (int i = DMS.SFS_GetStartSliderSampleIndex(); i <= DMS.SFS_GetEndSliderSampleIndex(); i++) {
            tempPosition = samplePositions[i];
            tempRotation = sampleRotations[i];
            samplePositions[i] = alternativeSamplePositions[i];
            sampleRotations[i] = alternativeSampleRotations[i];
            alternativeSamplePositions[i] = tempPosition;
            alternativeSampleRotations[i] = tempRotation;
        }
    }
}
