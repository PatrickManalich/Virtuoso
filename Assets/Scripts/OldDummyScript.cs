using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OldDummyScript : MonoBehaviour {

    //private int sampleCount;                        // The number of samples in the animation
    //private SpeedBandScript speedBandScript;        // The speed band script, used for caching
    //private PlayBandScript playBandScript;          // The play band script, used for caching
    //private OVRGrabber leftOVRGrabberScript;        // The OVR grabber script of the left hand anchor
    //private OVRGrabber rightOVRGrabberScript;       // The OVR grabber script of the right hand anchor
    //private SliderFieldScript sliderFieldScript;    // The slider field script, used for caching
    //private GloveScript leftGloveScript;            // The glove script of the left glove, used for caching
    //private RefineGuideScript refineGuideScript;    // The refine guide script, used for caching
    //private Vector3[] samplePositions;              // An array of the sample positions
    //private Quaternion[] sampleRotations;           // An array of the sample rotations
    //private int lastSampleIndex;                    // The index of the last sample, used for playing animation
    //private float lastT;                            // The last t, used for playing animation
    //private bool isPaused;                          // Has the animation been paused
    //private Vector3[] editedSamplePositions;        // An array of the edited sample positions
    //private Quaternion[] editedSampleRotations;     // An array of the edited sample rotations

    //public float animationSeconds;                  // How long the animation lasts in seconds
    //public GameObject speedBand;                    // The speed band of the slider field
    //public GameObject playBand;                     // The play band of the slider field
    //public GameObject sliderField;                  // The slider field Game Object, used for adjusting current slider
    //public GameObject leftHandAnchor;               // The left hand anchor Game Object
    //public GameObject rightHandAnchor;              // The right hand anchor Game Object
    //public GameObject leftGlove;                    // The left glove Game Object (you only need one)
    //public GameObject refineGuide;                  // The refine guide Game Object

    //private void Awake() {
    //    transform.position = new Vector3(-0.5f, -0.46f, 0.25f);
    //    transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    //    speedBandScript = speedBand.GetComponent<SpeedBandScript>();
    //    playBandScript = playBand.GetComponent<PlayBandScript>();
    //    sliderFieldScript = sliderField.GetComponent<SliderFieldScript>();
    //    leftOVRGrabberScript = leftHandAnchor.GetComponent<OVRGrabber>();
    //    rightOVRGrabberScript = rightHandAnchor.GetComponent<OVRGrabber>();
    //    leftGloveScript = leftGlove.GetComponent<GloveScript>();
    //    refineGuideScript = refineGuide.GetComponent<RefineGuideScript>();

    //    // Initialize sample positions and sample rotations
    //    sampleCount = (int)Mathf.Ceil(animationSeconds / speedBandScript.GetSampleRate()); // Ceil to make a nice, even number
    //    samplePositions = new Vector3[sampleCount];
    //    for (int i = 0; i < sampleCount; i++) { samplePositions[i] = transform.position; }
    //    sampleRotations = new Quaternion[sampleCount];
    //    for (int i = 0; i < sampleCount; i++) { sampleRotations[i] = transform.rotation; }

    //    lastSampleIndex = (sampleCount / 2) - 1;
    //    lastT = 0f;
    //    isPaused = false;

    //    editedSamplePositions = new Vector3[sampleCount];
    //    editedSampleRotations = new Quaternion[sampleCount];
    //}

    //private void Start() {
    //    sliderFieldScript.AdjustSlider("StartSlider", 0, 0f);
    //    sliderFieldScript.AdjustSlider("CurrentSlider", lastSampleIndex, 0f);
    //    sliderFieldScript.AdjustSlider("EndSlider", sampleCount - 1, 0f);
    //}

    ///* Coroutine that moves the dummy from its current sample to the next sample by lerping over t. Will move back to first sample after
    // * it has reached the last sample. Stores the information in global variables lastT and lastSampleIndex for the ability to pause and
    // * then play again from the same position. */
    //private IEnumerator MoveToNextSample() {
    //    if (lastSampleIndex < sliderFieldScript.GetEndSliderSampleIndex()) {    // If last sample index isn't out of range, continue playing
    //        if (!isPaused)
    //            lastT = 0f;
    //        else
    //            isPaused = false;
    //        while (lastT < 1) { // Lerp from t being 0 to t being 1
    //            lastT += Time.deltaTime / speedBandScript.GetSampleRate();
    //            transform.position = Vector3.Lerp(samplePositions[lastSampleIndex], samplePositions[lastSampleIndex + 1], lastT);
    //            transform.rotation = Quaternion.Lerp(sampleRotations[lastSampleIndex], sampleRotations[lastSampleIndex + 1], lastT);
    //            sliderFieldScript.AdjustSlider("CurrentSlider", lastSampleIndex, lastT);
    //            yield return null;
    //        }
    //        // When it has reached the next sample, increment last sample index and start MoveToNextSample again
    //        lastSampleIndex++;
    //        yield return StartCoroutine(MoveToNextSample());
    //    } else {    // If the last sample index is out of range, reset dummy to start slider positin and start MoveToNextSample again
    //        GoToStart();
    //        yield return StartCoroutine(MoveToNextSample());
    //    }
    //}

    ///* Coroutine that overwrites the samples every sample rate seconds. Will update and used last sample index to recursively call itself
    // * until it has reached the end slider sample index, in which case it will force release the left and right grabber. */
    //private IEnumerator OverwriteNextSample() {
    //    if (lastSampleIndex <= sliderFieldScript.GetEndSliderSampleIndex()) {
    //        editedSamplePositions[lastSampleIndex] = transform.position;
    //        editedSampleRotations[lastSampleIndex] = transform.rotation;
    //        sliderFieldScript.AdjustSlider("CurrentSlider", lastSampleIndex, 0f);
    //        lastSampleIndex++;
    //        yield return new WaitForSeconds(speedBandScript.GetSampleRate());
    //        yield return StartCoroutine(OverwriteNextSample());
    //    } else {
    //        leftOVRGrabberScript.ForceRelease(gameObject.GetComponent<OVRGrabbable>());
    //        rightOVRGrabberScript.ForceRelease(gameObject.GetComponent<OVRGrabbable>());
    //        yield return null;
    //    }
    //}

    ///* Refines each sample index in between the start slider sample index and the end slider sample index. Uses a Hermite curve to give
    // * ease-in ease-out functionality. Slices the work into two halves, one from the start slider sample index to the last sample index,
    // * and one from the last sample index to the end sample index. */
    //private void ApplyRefinement() {
    //    float refineT;
    //    float percent;
    //    int startSliderSampleIndex = sliderFieldScript.GetStartSliderSampleIndex(); // For caching
    //    int endSliderSampleIndex = sliderFieldScript.GetEndSliderSampleIndex();     // For caching

    //    // From range [start slider sample index, last sample index]
    //    for (int i = startSliderSampleIndex; i <= lastSampleIndex; i++) {
    //        percent = (i - startSliderSampleIndex) / ((float)lastSampleIndex - startSliderSampleIndex);
    //        refineT = Mathfx.Hermite(0.0f, 1.0f, percent);
    //        samplePositions[i] = Vector3.Lerp(samplePositions[i], transform.position, refineT);
    //        sampleRotations[i] = Quaternion.Lerp(sampleRotations[i], transform.rotation, refineT);
    //    }
    //    // From range (last sample index, end slider sample index]
    //    for (int i = lastSampleIndex + 1; i <= endSliderSampleIndex; i++) {
    //        percent = (i - lastSampleIndex - 1) / ((float)endSliderSampleIndex - lastSampleIndex - 1);
    //        refineT = Mathfx.Hermite(0.0f, 1.0f, percent);
    //        samplePositions[i] = Vector3.Lerp(transform.position, samplePositions[i], refineT);
    //        sampleRotations[i] = Quaternion.Lerp(transform.rotation, sampleRotations[i], refineT);
    //    }
    //}

    ///* Returns the sample count. */
    //public int GetSampleCount() { return sampleCount; }

    ///* Takes in a sample index and returns the sample position at that index. */
    //public Vector3 GetSamplePosition(int sampleIndex) { return samplePositions[sampleIndex]; }

    ///* Takes in a sample index and returns the sample rotation at that index. */
    //public Quaternion GetSampleRotation(int sampleIndex) { return sampleRotations[sampleIndex]; }

    ///* Takes in a sample index and a t and adjusts the position of the dummy based on the values. */
    //public void Adjust(int sampleIndex, float t) {
    //    lastT = t;
    //    lastSampleIndex = sampleIndex;
    //    transform.position = Vector3.Lerp(samplePositions[lastSampleIndex], samplePositions[lastSampleIndex + 1], lastT);
    //    transform.rotation = Quaternion.Lerp(sampleRotations[lastSampleIndex], sampleRotations[lastSampleIndex + 1], lastT);
    //}

    ///* Begin playing the animation by starting the MoveToNextSample() coroutine. */
    //public void StartPlaying() { StartCoroutine(MoveToNextSample()); }

    ///* Stop playing the animation by stopping the coroutine. */
    //public void StopPlaying() {
    //    StopAllCoroutines();
    //    isPaused = true;
    //}

    ///* Place dummy at the start slider position. */
    //public void GoToStart() {
    //    Adjust(sliderFieldScript.GetStartSliderSampleIndex(), 0f);
    //    sliderFieldScript.AdjustSlider("CurrentSlider", sliderFieldScript.GetStartSliderSampleIndex(), 0f);
    //}

    ///* Called once when the dummy is first grabbed. Depending on the state of the left glove (overwite or refine), will call the appropriate
    // * functions. */
    //public void GrabBegin() {
    //    if (!isPaused) {    // If it was playing when grabbed, stop playing and force the play band to pause
    //        StopPlaying();
    //        playBandScript.ForcePauseToggle();
    //    }

    //    if (leftGloveScript.isInOverwriteState()) {
    //        sliderFieldScript.AdjustSlider("StartSlider", lastSampleIndex, 0f);     // force the start slider to align
    //        lastT = 0f;
    //        StartCoroutine(OverwriteNextSample());
    //    } else    // Refining
    //        refineGuideScript.StartGuiding(transform.position);
    //}

    ///* Called once the dummy is let go. Depending on the state of the left glove (overwite or refine), will either overwrite samples with
    // * edited samples or call ApplyRefinement(). */
    //public void GrabEnd() {
    //    if (leftGloveScript.isInOverwriteState()) {
    //        StopAllCoroutines();
    //        int newEndSliderSampleIndex = lastSampleIndex - 1;    // the minus one comes from lastSampleIndex incrementing an extra time on last sample 
    //        for (int i = sliderFieldScript.GetStartSliderSampleIndex(); i <= newEndSliderSampleIndex; i++) {
    //            samplePositions[i] = editedSamplePositions[i];
    //            sampleRotations[i] = editedSampleRotations[i];
    //        }
    //        sliderFieldScript.AdjustSlider("EndSlider", newEndSliderSampleIndex, 0f);   // force the end slider to align 

    //        GoToStart();
    //    } else {    // Refining
    //        refineGuideScript.StopGuiding();
    //        ApplyRefinement();
    //    }

    //}

}
